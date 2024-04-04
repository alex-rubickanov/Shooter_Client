using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Sections")]
    [SerializeField] private MainMenuSection connectionSection;
    [SerializeField] private MainMenuSection weaponCatalogueSection;
    [SerializeField] private MainMenuSection optionsSection;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button weaponCatalogueButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        weaponCatalogueButton.onClick.AddListener(OnWeaponCatalogueButtonClicked);
        optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        connectionSection.Show();
        Hide();
    }

    private void OnWeaponCatalogueButtonClicked()
    {
        weaponCatalogueSection.Show();
        Hide();
    }

    private void OnOptionsButtonClicked()
    {
        optionsSection.Show();
        Hide();
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
