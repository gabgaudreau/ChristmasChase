/*
* @author Gabriel Gaudreau - ShowCanvas.cs
* @date Jan. 3rd 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;

public class ShowCanvas : MonoBehaviour {

    private GameObject _ui;

    /**
    * Start function. This function will be called when the Level is loaded.
    * This function will find the UI Canvas and display it.
    */
    void Start () {
        _ui = GameObject.Find("UI");
        if(_ui == null) {
            Debug.Log("Error finding UI. Exiting.");
            Application.Quit();
        }
        _ui.GetComponent<Canvas>().enabled = true;
	}
}
