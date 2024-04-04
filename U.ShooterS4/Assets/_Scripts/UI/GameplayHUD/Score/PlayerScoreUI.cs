using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI deathsText;
    
    public PlayerScore playerScore;

    public void Init(string playerName, PlayerScore _playerScore)
    {
        nameText.text = playerName;
        playerScore = _playerScore;
        UpdateScore();
    }

    public void UpdateScore()
    {
        killsText.text = playerScore.Kills.ToString();
        deathsText.text = playerScore.Deaths.ToString();
    }
}
