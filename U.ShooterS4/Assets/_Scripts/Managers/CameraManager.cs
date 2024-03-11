using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera mainMenuCamera;

    private void Start()
    {
        Instance = this;
    }
    
    public Camera GetGameplayCamera()
    {
        return gameplayCamera;
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
