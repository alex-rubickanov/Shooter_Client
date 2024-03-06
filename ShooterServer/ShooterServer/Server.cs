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

                if (clientsSockets.Count != 0)
                {
                    TransferData();
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
                serverSocket.Listen(10);
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
                
                //SendWelcomePacket(clientSocket);

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

                return;
            }
            SendConnectionPacket();
        }

        private void OnPlayerConnected()
        {
            SendConnectionPacket();
        }

        private void TransferData()
        {
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

        public void SendWelcomePacket(Socket clientSocket)
        {
            if (clientSocket == null) return;
            DebugLogPacket dp = new DebugLogPacket("Hi from server, dawg!", ServerData);
            clientSocket.Send(dp.Serialize());
        }

        public void SendDebugLogPacket(Socket clientSocket, string message)
        {
            if (clientSocket == null) return;
            DebugLogPacket dp = new DebugLogPacket(message, ServerData);
            clientSocket.Send(dp.Serialize());
        }

        public void SendConnectionPacket()
        {
            for (int i = 0; i < clientsSockets.Count; i++)
            {
                ConnectionPacket cp = new ConnectionPacket(clientsSockets.Count - 1, ServerData);
                clientsSockets[i].Send(cp.Serialize());
            }
        }

        public void SendDebugLogPacket(string message)
        {
            try
            {
                for (int i = 0; i < clientsSockets.Count; i++)
                    SendDebugLogPacket(clientsSockets[i], message);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.WouldBlock)
                    Console.Error.WriteLine(ex.Message);
            }
        }
    }
}