using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private List<Level> levels;
    
    public Level CurrentLevel { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var level in levels)
        {
            level.gameObject.SetActive(false);
        }
    }

    public void LoadLevel(int index)
    {
        if(CurrentLevel != null)
            CurrentLevel.gameObject.SetActive(false);
        
        CurrentLevel = levels[index];
        CurrentLevel.gameObject.SetActive(true);
        CameraManager.Instance.GameplayCamera = CurrentLevel.LevelCamera;
    }
}
