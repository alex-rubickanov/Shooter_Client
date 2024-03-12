using System;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [SerializeField] private int kills;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Kills"))
        {
            kills = PlayerPrefs.GetInt("Kills");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Debug.Log("Plus Kill");
            kills++;
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
        PlayerPrefs.SetInt("Kills", kills);
        PlayerPrefs.Save();
    }

    public void Clear()
    {
        kills = 0;
        PlayerPrefs.DeleteKey("Kills");
    }
}