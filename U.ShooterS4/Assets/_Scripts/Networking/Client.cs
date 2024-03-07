using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using ShooterNetwork;
using UnityEngine.Serialization;

public class Client : MonoBehaviour
{
    public static Client Instance;
    [SerializeField] private string desiredName; // TEST

    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    private Socket clientSocket;
    private List<PlayerClone> playerClones = new List<PlayerClone>();

    // Callbacks
    private AsyncCallback connectServerCallback;
    // Packets Events
    public Action<DebugLogPacket> OnDebugLogPacketReceived;

    [SerializeField] private PlayerClone clonePrefab;

    [Header("-----TIMINGS-----")]
    [SerializeField] private float moveSendRate = 0.1f;
    public float MOVE_SEND_RATE => moveSendRate;

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
        playerData = new PlayerData(desiredName);
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
                    case PacketType.Connection:
                        ReadConnectionPacket(buffer);
                        break;
                    case PacketType.Move:
                        MovePacket mp = new MovePacket().Deserialize(buffer);
                        int id = int.Parse(mp.DataHolder.ID);
                        Debug.Log(id + "moves!");
                        if (playerClones.Count > id)
                        {
                            playerClones[id].transform.position = new Vector3(mp.Position.x, 0, mp.Position.y);
                        }

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
                Debug.Log(ex.Message);
            }
        }
    }

    public void SendPacket(BasePacket packet)
    {
        clientSocket.Send(packet.Serialize());
    }

    public void ReadConnectionPacket(byte[] buffer)
    {
        ConnectionPacket cp = new ConnectionPacket().Deserialize(buffer);
        Debug.Log("Player connected! ID: " + cp.PlayersAmount);

        string name = playerData.Name;
        playerData = new PlayerData(name, (cp.PlayersAmount).ToString());
        int clonesToSpawn = cp.PlayersAmount - playerClones.Count;
        for (int i = 0; i < clonesToSpawn; i++)
        {
            PlayerClone clone = Instantiate(clonePrefab);
            playerClones.Add(clone);
        }
    }
}