using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private PlayerClone clonePrefab;
    
    public static Client Instance;

    private Socket clientSocket;
    private List<PlayerClone> playerClones = new List<PlayerClone>();

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

    public void ConnectToServer(string ip = "127.00.01")
    {
        try
        {
            Debug.Log("Connecting to server...");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), 3000));
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
        SendConnectionPacket();
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
                    case PacketType.ConnectionPacket:
                        Debug.LogWarning($"{gameObject.name}" + " received a ConnectionPacket packet");
                        ConnectionPacket cp = new ConnectionPacket().Deserialize(buffer);
                        PlayerClone clone = Instantiate(clonePrefab);
                        playerClones.Add(clone);
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
    
    public void SendConnectionPacket()
    {
        clientSocket.Send(new ConnectionPacket(clientSocket).Serialize());
    }
    
    public void SendPacket(BasePacket packet)
    {
        clientSocket.Send(packet.Serialize());
    }
}