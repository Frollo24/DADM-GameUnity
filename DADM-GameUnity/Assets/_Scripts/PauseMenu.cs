using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;

    public GameObject pausePanel;
    public GameObject pauseMenu;

    public void Pause()
    {
        pausePanel.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = false;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        FindObjectOfType<ResultMenu>().gameObject.SetActive(false);
        ResultMenu.HasWon = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Pause();
        }
    }
}
