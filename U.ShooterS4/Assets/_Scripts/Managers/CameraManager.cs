using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private Camera mainMenuCamera;

    public Camera GameplayCamera;

    private void Awake()
    {
        Instance = this;
    }

    public Camera GetMainMenuCamera()
    {
        return mainMenuCamera;
    }

    public void SetGameplayCamera()
    {
        GameplayCamera.gameObject.SetActive(true);
        mainMenuCamera.gameObject.SetActive(false);
    }

    public void SetMainMenuCamera()
    {
        if (GameplayCamera != null)
        {
            GameplayCamera.gameObject.SetActive(false);
        }

        mainMenuCamera.gameObject.SetActive(true);
    }
}