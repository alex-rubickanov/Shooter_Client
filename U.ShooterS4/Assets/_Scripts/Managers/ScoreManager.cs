using System.Collections.Generic;
using ShooterNetwork;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    public static ScoreManager Instance;
    public Dictionary<PlayerData, PlayerScore> playerScores = new Dictionary<PlayerData, PlayerScore>();
    
    
    [SerializeField] private ScoreUI scoreUI;
    
    private bool isScoreTabOpen = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Client.Instance.OnDeathPacketReceived += ReadDeathPacket;
        CloseOpenScoreTab();
    }

    private void ReadDeathPacket(DeathPacket packet)
    {
        PlayerData killerData = Client.Instance.GetPlayerDataByID(packet.KillerID);
        PlayerData victimData = (PlayerData)packet.DataHolder;
        
        AddKill(killerData, victimData);
    }

    public void AddKill(PlayerData killerData, PlayerData victimData)
    {
        PlayerScore killer = playerScores[killerData];
        PlayerScore victim = playerScores[victimData];
        
        killer.Kills++;
        victim.Deaths++;
        
        scoreUI.UpdateScore(killerData);
        scoreUI.UpdateScore(victimData);
    }

    public void AddPlayer(PlayerData playerData)
    {
        if(playerScores.ContainsKey(playerData)) return;
        playerScores.Add(playerData, new PlayerScore());
        scoreUI.AddPlayerScoreUI(playerData, playerScores[playerData]);
    }

    private void OnEnable()
    {
        inputReader.OnOpenScoreTabEvent += CloseOpenScoreTab;
    }
    
    private void OnDisable()
    {
        inputReader.OnOpenScoreTabEvent -= CloseOpenScoreTab;
    }

    private void CloseOpenScoreTab()
    {
        if (isScoreTabOpen)
        {
            isScoreTabOpen = false;
            scoreUI.gameObject.SetActive(false);
        } 
        else
        {
            isScoreTabOpen = true;
            scoreUI.gameObject.SetActive(true);
        }
    }
}

public class PlayerScore
{
    public int Kills = 0;
    public int Deaths = 0;
}