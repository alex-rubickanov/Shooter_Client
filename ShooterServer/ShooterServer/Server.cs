namespace ShooterServer
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    public class Server
    {
        private int port;
        private Socket serverSocket;
        private List<Socket> clientsSockets = new List<Socket>();

        private bool isServerStarted = false;
        private bool canAccept = true;

        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            InitializeServer();

            while (true)
            {
                if (!isServerStarted) return;

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
                isServerStarted = true;
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
                clientsSockets.Add(clientSocket);
                Console.WriteLine($"Client connected: {clientSocket.LocalEndPoint}");
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.WouldBlock)
                {
                    Console.Error.WriteLine("Exception while accepting clients.");
                    Console.WriteLine(ex);
                }
            }
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
                            SocketError.ConnectionAborted || // Server handle disconnection only in build
                            ex.SocketErrorCode == SocketError.ConnectionReset)
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
    }
}