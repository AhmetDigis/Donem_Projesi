using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;

    
    void Start()
    {

    }

    public void win()
    {
        Cursor.lockState = CursorLockMode.None;
        winPanel.SetActive(true);
        Time.timeScale = 0;

    }

    public void gameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;

    }

    public void playAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);

    }
    public void mainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
