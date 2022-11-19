using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        GameController.S_instance.AudioManager.PlayMusic(true);
        GameController.S_instance.LastLevel.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        GameController.S_instance.AudioManager.PlayMusic(false);
    }

    public void OptionsButton()
    {

    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
