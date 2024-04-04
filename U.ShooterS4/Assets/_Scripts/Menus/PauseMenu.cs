using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject settingsUI;
    [SerializeField] bool isPaused;
    [SerializeField] string MainMenuName;
    void Start()
    {
        menuUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }
    }

    void PauseGame()
    {
        isPaused = true;
        menuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        isPaused = false;
        menuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResumeButton()
    {
        isPaused = false;
        menuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(MainMenuName);
    }

    public void ReturnToMainPause()
    {
        settingsUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void OptionButton()
    {
        settingsUI.SetActive(true);
        menuUI.SetActive(false);
    }
}

