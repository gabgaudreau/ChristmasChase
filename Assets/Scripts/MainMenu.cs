/*
* @author Gabriel Gaudreau - MainMenu.cs
* @date Jan. 2nd 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    /**
    * This function is called when clicking the Play button in the menu.
    */
    public void OnClickPlay() {
        GameManager._gm.StartGame();
        SceneManager.LoadScene(1); // Game Scene
    }

    /**
    * This function is called when clicking the How to Play button in the menu.
    */
    public void OnClickHowTo() {
        SceneManager.LoadScene(2); // How To Play Scene
    }

    /**
    * This function is called when clicking the exit button in the menu.
    */
    public void OnClickExit() {
        Application.Quit();
    }

    /**
    * This function is called when clicking a back button in the menu.
    * This function will explicitely destroy all items using the DontDestroyOnLoad Script.
    */
    public void OnClickBackToMenu() {
        foreach (DontDestroyOnLoad o in FindObjectsOfType<DontDestroyOnLoad>()) {
            Destroy(o.gameObject);
        }
        SceneManager.LoadScene(0); // Main Menu Scene
    }
}
