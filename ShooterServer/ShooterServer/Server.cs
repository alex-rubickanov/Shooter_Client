using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ShooterNetwork;

namespace ShooterServer
{
    public class Server
    {
        private Socket serverSocket;
        private List<Socket> clientsSockets = new List<Socket>();

        private int port;

        private bool canAccept = true;

        private readonly ServerData ServerData = new ServerData("Server", "0");
        private const int MAX_PLAYERS = 4;

        private List<PlayerData> playersData = new List<PlayerData>();

        private const int PLAYERS_TO_PLAY = 3;

        private bool isGameStarted = false;

        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            InitializeServer();

            while (true)
            {
                if (canAccept)
                {
                    AcceptClients();
                }

                TransferData();
                if (clientsSockets.Count >= PLAYERS_TO_PLAY && !isGameStarted)
                {
                    StartGame();
                }
            }
        }

        private void InitializeServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Blocking = false;
                serverSocket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));
                serverSocket.Listen(4);
                Console.WriteLine("Server started!");
                Console.WriteLine("Waiting for client to connect...");
            }
            catch (SocketException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private void AcceptClients()
        {
            try
            {
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Blocking = false;
                clientsSockets.Add(clientSocket);
                Console.WriteLine($"Client connected!");

                AssignClientID(clientSocket, clientsSockets.Count);

                if (clientsSockets.Count == MAX_PLAYERS)
                {
                    canAccept = false;
                    Console.WriteLine("Server is full!");
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.WouldBlock)
                {
                    Console.Error.WriteLine("Exception while accepting client.");
                    Console.WriteLine(ex);
                }
            }
        }


        private void TransferData()
        {
            if (clientsSockets.Count == 0) return;

            for (int i = 0; i < clientsSockets.Count; i++)
            {
                if (clientsSockets[i].Available <= 0) continue;
                try
                {
                    byte[] buffer = new byte[clientsSockets[i].Available];
                    clientsSockets[i].Receive(buffer);

                    for (int j = 0; j < clientsSockets.Count; j++)
                    {
                        if (i == j) continue;
                        clientsSockets[j].Send(buffer);
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                    {
                        if (ex.SocketErrorCode ==
                            SocketError
                                .ConnectionAborted || // BUG: If clientSocket.Available = 0 => disconnections are not detected
                            ex.SocketErrorCode ==
                            SocketError.ConnectionReset) // TODO: Heartbeat to check if client is still connected
                        {
                            Console.WriteLine("Client disconnected!");
                            clientsSockets.RemoveAt(i);
                        }
                        else
                        {
                            Console.Error.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }


        public void AssignClientID(Socket clientSocket, int id)
        {
            AssignIDPacket assignIDPacket = new AssignIDPacket(id.ToString(), ServerData);
            clientSocket.Send(assignIDPacket.Serialize());
        }

        public void StartGame()
        {
            canAccept = false;
            Console.WriteLine("Game started!");
            isGameStarted = true;
            StartGamePacket startGamePacket = new StartGamePacket(ServerData);
            for (int i = 0; i < clientsSockets.Count; i++)
            {
                try
                {
                    clientsSockets[i].Send(startGamePacket.Serialize());
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}