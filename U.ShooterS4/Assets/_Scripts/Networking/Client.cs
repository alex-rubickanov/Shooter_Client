using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using ShooterNetwork;

public class Client : MonoBehaviour
{
    public static Client Instance;
    [SerializeField] private string desiredName; // TEST

    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    private Socket clientSocket;
    private Dictionary<string, PlayerClone> playerClones = new Dictionary<string, PlayerClone>();

    // Packets Events
    public event Action<DebugLogPacket> OnDebugLogPacketReceived;
    public event Action<MovePacket> OnMovePacketReceived;


    [SerializeField] private PlayerClone clonePrefab;

    public event Action OnIDAssigned;

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
    }

    private void Update()
    {
        if (!clientSocket.Connected)
            return;

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

                    case PacketType.AssignID:
                        AssignIDFromPacket(buffer);
                        break;

                    case PacketType.PlayerPawnSpawn:
                        SpawnPlayerClone(buffer);
                        break;

                    case PacketType.Move:
                        MovePacket mp = new MovePacket().Deserialize(buffer);
                        OnMovePacketReceived?.Invoke(mp);
                        break;

                    case PacketType.PlayerData:
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

    private void SpawnPlayerClone(byte[] buffer)
    {
        PawnSpawnPacket psp = new PawnSpawnPacket().Deserialize(buffer);
        PlayerClone pc = Instantiate(clonePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        PlayerData cloneData = new PlayerData(psp.DataHolder.Name, psp.DataHolder.ID);
        pc.SetCloneData(cloneData);
        pc.gameObject.name = psp.DataHolder.Name + " ID:" + psp.DataHolder.ID;
        playerClones.Add(psp.DataHolder.ID, pc);
    }


    private void AssignIDFromPacket(byte[] buffer)
    {
        AssignIDPacket aidp = new AssignIDPacket().Deserialize(buffer);
        playerData = new PlayerData(desiredName, aidp.ID);
        if (playerData.ID == "0")
        {
            Debug.LogError("Player ID is 0. Possible IDs are 1-4.");
        }

        OnIDAssigned?.Invoke();
    }

    public void SendPacket(BasePacket packet)
    {
        Debug.Log("Sending packet to server! " + packet.Type);
        clientSocket.Send(packet.Serialize());
    }
}