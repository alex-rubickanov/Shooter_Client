using System;
using ShooterNetwork;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [SerializeField] private int kills;
    private bool killedAlex;
    private bool killedMustafa;

    public bool KilledAlex => killedAlex;
    public bool KilledMustafa => killedMustafa;
    
    private bool usedCheats = false;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Kills"))
        {
            kills = PlayerPrefs.GetInt("Kills");
        }

        if (PlayerPrefs.HasKey("MustafaKilled"))
        {
            killedMustafa = Convert.ToBoolean(PlayerPrefs.GetInt("MustafaKilled"));
        }

        if (PlayerPrefs.HasKey("AlexKilled"))
        {
            killedAlex = Convert.ToBoolean(PlayerPrefs.GetInt("AlexKilled"));
        }

        Client.Instance.OnDeathPacketReceived += CheckKill;
    }

    private void CheckKill(DeathPacket packet)
    {
        if (packet.KillerID.ToString() == Client.Instance.PlayerData.ID)
        {
            AddKill();
            Debug.Log(packet.DataHolder.Name);
            if (packet.DataHolder.Name == "Alex")
            {
                Debug.Log("ALEX KILLED");
                killedAlex = true;
            }
            else if (packet.DataHolder.Name == "Mustafa")
            {
                Debug.Log("MUSTAFA KILLED");
                killedMustafa = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Debug.Log("Plus Kill");
            usedCheats = true;
            killedAlex = true;
            killedMustafa = true;
            kills = kills + 100000;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void AddKill()
    {
        kills++;
    }

    public int GetKills()
    {
        return kills;
    }

    private void OnApplicationQuit()
    {
        if (usedCheats) return;
        PlayerPrefs.SetInt("Kills", kills);
        PlayerPrefs.SetInt("MustafaKilled", killedMustafa ? 1 : 0);
        PlayerPrefs.SetInt("AlexKilled", killedAlex ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Clear()
    {
        kills = 0;
        killedAlex = false;
        killedMustafa = false;
        PlayerPrefs.DeleteKey("Kills");
    }
}