using System;
using System.Collections;
using UnityEngine;

public class StartGameMenu : MonoBehaviour
{
    public static StartGameMenu Instance;
    
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private MainMenuSection connectionSection;
    [SerializeField] private MainMenuSection weaponCatalogueSection;
    [SerializeField] private MainMenuSection optionsSection;
    [SerializeField] private WaitingRoom waitingRoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(Client.Instance.showStartGameMenu)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        
        CameraManager.Instance.SetGameplayCamera();

    }

    public void Open()
    {
        gameObject.SetActive(true);
        
        mainMenu.Show();
        connectionSection.Hide();
        weaponCatalogueSection.Hide();
        optionsSection.Hide();
        waitingRoom.Hide();
        
        CameraManager.Instance.SetMainMenuCamera();
    }

    public void OpenWaitingRoom(string name, string ip)
    {
        waitingRoom.Show();
        StartCoroutine(OpenWaitingRoomCoroutine(name, ip));
    }
    
    private IEnumerator OpenWaitingRoomCoroutine(string name, string ip)
    {
        yield return new WaitForSeconds(1.0f);
        Client.Instance.ConnectToServer(name, ip);
    }

    public void ShowMessageWaitingRoom(string errorMessage)
    {
        waitingRoom.ShowError(errorMessage);
    }
}