using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button disconnectButton;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private FloatReference sfxVolume;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        sfxSlider.value = sfxVolume.Value;
        sfxSlider.onValueChanged.AddListener((value) => sfxVolume.Value = value);
        
        disconnectButton.onClick.AddListener(Client.Instance.Disconnect);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
