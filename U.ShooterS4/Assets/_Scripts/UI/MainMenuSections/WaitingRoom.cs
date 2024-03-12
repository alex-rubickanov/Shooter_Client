using TMPro;
using UnityEngine;

public class WaitingRoom : MainMenuSection
{
    [SerializeField] private TextMeshProUGUI windowText;

    private string defaultText; 

    protected override void Start()
    {
        base.Start();
        defaultText = windowText.text;
        
        backButton.onClick.AddListener(ResetWindow);
        
        backButton.gameObject.SetActive(false);
    }


    public void ShowError(string errorMessage)
    {
        windowText.text = errorMessage;
        backButton.gameObject.SetActive(true);
    }

    public void ResetWindow()
    {
        windowText.text = defaultText;
        backButton.gameObject.SetActive(false);
    }
}