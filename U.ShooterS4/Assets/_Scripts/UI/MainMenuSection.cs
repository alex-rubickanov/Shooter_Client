using UnityEngine;
using UnityEngine.UI;

public class MainMenuSection : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [Header("Buttons")]
    [SerializeField] protected Button backButton;

    protected virtual void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
    }
    
    private void OnBackButtonClicked()
    {
        mainMenu.Show();
        Hide();
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
