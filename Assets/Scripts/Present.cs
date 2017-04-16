/*
* @author Gabriel Gaudreau - Present.cs
* @date Jan. 5th 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;

public class Present : MonoBehaviour {

    /**
    * This function will execute when a present is instantiated and 
    * will destroy the present after a 15.0 second delay.
    */
    void Start() {
        Destroy(gameObject, 15.0f);
    }
}
