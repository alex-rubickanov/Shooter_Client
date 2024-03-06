using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client Instance;

    private Socket clientSocket;

    // Callbacks
    private AsyncCallback connectServerCallback;

    // Packets Events
    public Action<DebugLogPacket> OnDebugLogPacketReceived;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        try
        {
            Debug.Log("Connecting to server...");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 3000));
            clientSocket.Blocking = false;
        }
        catch (SocketException ex)
        {
            if (ex.SocketErrorCode == SocketError.ConnectionRefused)
            {
                Debug.LogError("Server is not running!");
            }
            else
            {
                Debug.LogError("Error while connecting to server!");
                Debug.LogError(ex.Message);
            }

            return;
        }

        Debug.Log($"Connected to server!  {clientSocket.LocalEndPoint.ToString()}");
    }

    private void Update()
    {
        if (!clientSocket.Connected)
            return;

        //clientSocket.Send(new DebugLogPacket("Hello from client!").Serialize());

        ReceiveData();
    }

    private void ReceiveData()
    {
        if (clientSocket.Available < 0)
            return;

        try
        {
            byte[] buffer = new byte[clientSocket.Available];
            clientSocket.Receive(buffer);

            while (BasePacket.DataRemainingInBuffer(buffer.Length))
            {
                BasePacket bp = new BasePacket().Deserialize(buffer);

                switch (bp.Type)
                {
                    case PacketType.None:
                        Debug.LogWarning($"{gameObject.name}" + " received a None packet");
                        break;
                    case PacketType.DebugLog:
                        DebugLogPacket dlp = new DebugLogPacket().Deserialize(buffer);
                        OnDebugLogPacketReceived?.Invoke(dlp);
                        break;
                    default:
                        Debug.LogWarning($"{gameObject.name}" + " received an unknown packet");
                        break;
                }
            }
        }
        catch (SocketException ex)
        {
            if (ex.SocketErrorCode != SocketError.WouldBlock)
            {
                print(ex.Message);
            }
        }
    }

    private void OnEnable()
    {
        OnDebugLogPacketReceived += Test;
    }

    private void OnDisable()
    {
        OnDebugLogPacketReceived -= Test;
    }

    private void Test(DebugLogPacket obj)
    {
        Debug.Log($"Message received: {obj.Message}");
    }
    
    public void SendPacket(BasePacket packet)
    {
        clientSocket.Send(packet.Serialize());
    }
}