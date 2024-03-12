using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera mainMenuCamera;

    private void Awake()
    {
        Instance = this;
    }
    
    public Camera GetGameplayCamera()
    {
        return gameplayCamera;
    }

    public Camera GetMainMenuCamera()
    {
        return mainMenuCamera;
    }

    public void SetGameplayCamera()
    {
        gameplayCamera.gameObject.SetActive(true);
        mainMenuCamera.gameObject.SetActive(false);
    }
    
    public void SetMainMenuCamera()
    {
        gameplayCamera.gameObject.SetActive(false);
        mainMenuCamera.gameObject.SetActive(true);
    }
}
