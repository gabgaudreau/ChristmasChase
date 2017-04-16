/*
* @author Gabriel Gaudreau - MouseControl.cs
* @date Jan. 1st 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour {
    [SerializeField]
    float _mouseSens;
    [SerializeField]
    Marker _markerPrefab;

    private Vector3 playerPos;
    private GameObject _player;
    
    /**
    * Start function. This function will find the player object and keep it in memory.
    */
    void Start() {
        _player = GameObject.Find("Player");
        if (_player == null) {
            Debug.Log("Error finding Player. Exiting.");
            Application.Quit();
        }
    }
    
    /**
    * Update function. This function will repeat once per frame.
    * This function will handle camera movement as well as player movement using move position and input
    * To give the proper position for the navigation agent to move properly.
    * This function uses ray casting to find a walkable plane for the user to click to move.
    */
    void Update () {
        // Input.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Left click or middle click to move camera around.
        if (Input.GetMouseButton(0) || Input.GetMouseButton(2)) {
            transform.Translate(new Vector3(mouseX, 0.0f, mouseY) * _mouseSens, Space.World);
        }

        // Hold space to keep camera on player (camera follow mode).
        if (Input.GetButton("Space")) {
            transform.position = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z + 20.0f);
        }

        // Right click to move.
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Walkable" || hit.collider.tag == "RedPresent" || hit.collider.tag == "GreenPresent" || hit.collider.tag == "GoldPresent" || hit.collider.tag == "BluePresent" || hit.collider.tag == "WhitePresent") {
                    _player.GetComponent<Player>().SetDestination(hit.point);
                    Instantiate(_markerPrefab, hit.point + new Vector3(0.0f, 0.1f, 0.0f), _markerPrefab.transform.rotation);
                }
            }
        }
    }
}
