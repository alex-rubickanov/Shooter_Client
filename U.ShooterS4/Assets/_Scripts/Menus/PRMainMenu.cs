using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PRMainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject AudioUI;
    [SerializeField] GameObject DisplayUI;
    [SerializeField] string level;
    // Start is called before the first frame update

    private void Start()
    {
        settingsUI.SetActive(false);
    }
    public void StartGame()
    {
        Debug.Log("GameStarted");
        SceneManager.LoadScene(level);
        AudioUI.SetActive(false);
        DisplayUI.SetActive(false);
    }

    public void Settings()
    {
        settingsUI.SetActive(true);
        AudioUI.SetActive(false);
        DisplayUI.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void AudioSetting()
    {
        AudioUI.SetActive(true);
        DisplayUI.SetActive(false);
    }

    public void DisplaySetting()
    {
        DisplayUI.SetActive(true);
        AudioUI.SetActive(false);
    }

    public void ReturnToMain()
    {
        settingsUI.SetActive(false);
    }
}
