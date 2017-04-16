/*
* @author Gabriel Gaudreau - TrapController.cs
* @date Jan. 2nd 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour {
    [SerializeField]
    GameObject trap;
    [SerializeField]
    float distance;
    private bool _activated, _moving; //activated - true = low, false = high
    private float _direction, _high, _low;

    /**
    * Start function. This function will run at the moment this run starts executing.
    * This function will set the values for _high and _low. 
    */
    void Start() {
        _high = trap.transform.position.y;
        _low = _high - distance;
    }

    /**
    * This function will return the _moving boolean
    * @return a boolean value : _moving
    */
    public bool GetMoving() {
        return _moving;
    }

    /*
    * This function will activate a trap.
    */
    public void Activate() {
        CheckState();
        _moving = true;
    }

    /**
    * This function will take the _activated boolean, check its value 
    * and change the value of direction appropriately.
    */
    void CheckState() {
        _direction = (_activated) ? 1.0f : -1.0f;
    }

    /**
    * Update function. This function will execute once per frame.
    * This function will make the traps move up and down once they are activated.
    */
    void Update() {
        if ((_moving && trap.transform.position.y > _low) || (_moving && trap.transform.position.y < _high)) {
            trap.transform.Translate(Vector3.up * 2.25f * Time.deltaTime * _direction);
        }
        if(trap.transform.position.y < _low) {
            _activated = true;
            _moving = false;
        }
        if(trap.transform.position.y > _high) {
            _activated = false;
            _moving = false;
        }
    }
}
