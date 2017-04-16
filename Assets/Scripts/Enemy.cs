/*
* @author Gabriel Gaudreau - Enemy.cs
* @date Jan. 2nd 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private NavMeshAgent _navAgent;
    private GameObject _player;

    /**
    * Start function. This function will find the player object and keep a reference to it.
    */
	void Start () {
        _navAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player");
        if(_player == null) {
            Debug.Log("Error finding Player. Exiting.");
            Application.Quit();
        }
	}
	
    /**
    * This function executes on a different time frame than Update() and 
    * will make the associated enemy move towards the player using the navigation system.
    */
	void FixedUpdate () {
        // Stops all enemies to allow the player to properly reposition during round transition.
        if (GameManager._gm.GetChangingRounds()) {
            _navAgent.Stop();
        }
        _navAgent.SetDestination(_player.transform.position);
    }
}
