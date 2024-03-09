using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using ShooterNetwork;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public static Client Instance;

    private string username;

    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    private Socket clientSocket;
    private Dictionary<string, PlayerClone> playerClones = new Dictionary<string, PlayerClone>();

    // Packets Events
    public event Action<DebugLogPacket> OnDebugLogPacketReceived;
    public event Action<MovePacket> OnMovePacketReceived;
    public event Action<AimPacket> OnAimPacketReceived;
    public event Action<EquipWeaponPacket> OnEquipWeaponPacketReceived;
    public event Action<FireBulletPacket> OnFireBulletPacketReceived;
    public event Action<ReloadPacket> OnReloadPacketReceived;
    public event Action<HitPacket> OnHitPacketReceived;
    public event Action<DeathPacket> OnDeathPacketReceived;
    public event Action<StartGamePacket> OnStartGamePacketReceived;


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
            Destroy(Instance);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 1)
        {
            Invoke(nameof(ConnectToServer), 1.0f);
        }
    }

    public void ConnectToServer(string name, string ip)
    {
        username = name;
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
        if (clientSocket == null || !clientSocket.Connected)
            return;

        ReceiveData();
    }

    private void ReceiveData()
    {
        if (!clientSocket.Connected || clientSocket.Available <= 0)
            return;

        try
        {
            byte[] buffer = new byte[clientSocket.Available];
            clientSocket.Receive(buffer);
            BasePacket.Reset();

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
                        Debug.Log(dlp.Message);
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

                    case PacketType.Aim:
                        AimPacket ap = new AimPacket().Deserialize(buffer);
                        OnAimPacketReceived?.Invoke(ap);
                        break;

                    case PacketType.EquipWeapon:
                        EquipWeaponPacket ewp = new EquipWeaponPacket().Deserialize(buffer);
                        OnEquipWeaponPacketReceived?.Invoke(ewp);
                        break;

                    case PacketType.FireBullet:
                        Debug.Log("Received FireBulletPacket");
                        FireBulletPacket fbp = new FireBulletPacket().Deserialize(buffer);
                        OnFireBulletPacketReceived?.Invoke(fbp);
                        break;

                    case PacketType.Reload:
                        ReloadPacket rp = new ReloadPacket().Deserialize(buffer);
                        OnReloadPacketReceived?.Invoke(rp);
                        break;

                    case PacketType.Hit:
                        HitPacket hp = new HitPacket().Deserialize(buffer);
                        OnHitPacketReceived?.Invoke(hp);
                        break;

                    case PacketType.Death:
                        DeathPacket dp = new DeathPacket().Deserialize(buffer);
                        OnDeathPacketReceived?.Invoke(dp);

                        playerClones.Remove(dp.DataHolder.ID);
                        break;

                    case PacketType.StartGame:
                        StartGamePacket sgp = new StartGamePacket().Deserialize(buffer);
                        OnStartGamePacketReceived?.Invoke(sgp);
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
        if (!playerClones.ContainsKey(psp.DataHolder.ID))
        {
            Vector3 pos = new Vector3(psp.Position.X, 0, psp.Position.Y);
            PlayerClone pc = Instantiate(clonePrefab, pos, Quaternion.identity);
            PlayerData cloneData = new PlayerData(psp.DataHolder.Name, psp.DataHolder.ID);
            pc.SetCloneData(cloneData);
            pc.gameObject.name = psp.DataHolder.Name + " ID:" + psp.DataHolder.ID;
            playerClones.Add(psp.DataHolder.ID, pc);
        }
    }


    private void AssignIDFromPacket(byte[] buffer)
    {
        AssignIDPacket aidp = new AssignIDPacket().Deserialize(buffer);
        playerData = new PlayerData(username, aidp.ID);
        if (playerData.ID == "0")
        {
            Debug.LogError("Player ID is 0. Possible IDs are 1-4.");
        }

        OnIDAssigned?.Invoke();
    }

    public void SendPacket(BasePacket packet)
    {
        //Debug.Log(gameObject.name + "Sending packet to server! " + packet.Type);
        clientSocket.Send(packet.Serialize());
    }

    public string GetPlayerNameByID(string id)
    {
        if(id == playerData.ID)
        {
            return playerData.Name;
        }
        
        if (playerClones.TryGetValue(id, out PlayerClone clone))
        {
            return clone.cloneData.Name;
        }

        return "id " + id;
    }
}