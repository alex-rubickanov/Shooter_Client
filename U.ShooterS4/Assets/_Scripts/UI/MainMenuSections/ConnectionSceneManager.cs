using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionSceneManager : MainMenuSection
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField ipInputField;

    [SerializeField] private Button connectButton;


    private new void Start()
    {
        base.Start();

        connectButton.onClick.AddListener(OnConnectButtonClicked);
    }

    private void OnConnectButtonClicked()
    {
        if (string.IsNullOrEmpty(nameInputField.text) ||
            string.IsNullOrEmpty(ipInputField.text) && !Client.Instance.disableServerConnection)
        {
            return;
        }


        if (!Client.Instance.disableServerConnection)
        {
            StartGameMenu.Instance.OpenWaitingRoom(nameInputField.text, ipInputField.text);
        }
        else
        {
            StartGameMenu.Instance.Close();
        }

        Hide();
    }
}