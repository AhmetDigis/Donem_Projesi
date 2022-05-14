using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public GameObject exitPanel;
    void Start()
    {
        
    }

    public void gameStart(){
        SceneManager.LoadScene(1);
    }

    public void exit(){
        
        exitPanel.SetActive(true);
        
    }

    public void exitMenu(string answer){
        switch(answer) {

            case "yes":
            Application.Quit();
            break;
            
            case "no":
            exitPanel.SetActive(false);
            break;


        }  
        
    }
}
