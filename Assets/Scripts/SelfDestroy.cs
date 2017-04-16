/*
* @author Gabriel Gaudreau - SelfDestroy.cs
* @date Jan. 5th 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

    /**
    * This function will destroy its associated gameobject after a 3.0 second delay.
    */
    void Start() {
        Destroy(gameObject, 3.0f);
    }
}
