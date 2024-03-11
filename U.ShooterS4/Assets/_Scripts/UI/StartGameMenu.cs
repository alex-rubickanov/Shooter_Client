using System;
using UnityEngine;

public class StartGameMenu : MonoBehaviour
{
    public static StartGameMenu Instance;
    
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private MainMenuSection connectionSection;
    [SerializeField] private MainMenuSection weaponCatalogueSection;
    [SerializeField] private MainMenuSection optionsSection;

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
    }

    public void Open()
    {
        gameObject.SetActive(true);
        
        mainMenu.Show();
        connectionSection.Hide();
        weaponCatalogueSection.Hide();
        optionsSection.Hide();
    }
}