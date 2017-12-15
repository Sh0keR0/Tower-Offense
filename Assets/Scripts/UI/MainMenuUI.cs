using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public GameObject MainMenu, CreditMenu;

    public void OnStartGamePressed()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnCreditsPressed()
    {
        MainMenu.SetActive(false);
        CreditMenu.SetActive(true);
    }

    public void OnExitGamePressed()
    {
        Application.Quit();
    }


    public void OnBackToMainMenuPressed()
    {
        MainMenu.SetActive(true);
        CreditMenu.SetActive(false);
    }
}
