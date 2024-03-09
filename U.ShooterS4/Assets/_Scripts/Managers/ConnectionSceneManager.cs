using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField ipInputField;

    [SerializeField] private Button connectButton;


    private void Start()
    {
        if (Client.Instance.disableServerConnection)
        {
            gameObject.SetActive(false);
        }
        connectButton.onClick.AddListener(OnConnectButtonClicked);
    }

    private void OnConnectButtonClicked()
    {
        if (string.IsNullOrEmpty(nameInputField.text) || string.IsNullOrEmpty(ipInputField.text))
        {
            return;
        }

        gameObject.SetActive(false);
        Client.Instance.ConnectToServer(nameInputField.text, ipInputField.text);
    }
}