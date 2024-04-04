using System;
using System.Collections.Generic;
using ShooterNetwork;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private RectTransform playerScoreContainer;
    [SerializeField] private PlayerScoreUI playerScoreUI;
    private Dictionary<PlayerData, PlayerScoreUI> playerScoreUIs = new Dictionary<PlayerData, PlayerScoreUI>();


    public void UpdateScore(PlayerData playerData)
    {
        playerScoreUIs[playerData].UpdateScore();
    }

    private void Clear()
    {
        foreach (var playerScore in playerScoreUIs)
        {
            Destroy(playerScore.Value.gameObject);
        }
    }

    public void AddPlayerScoreUI(PlayerData playerData, PlayerScore playerScore)
    {
        PlayerScoreUI ps = Instantiate(playerScoreUI, playerScoreContainer);
        ps.Init(playerData.Name, playerScore);
        playerScoreUIs.Add(playerData, ps);
    }
}