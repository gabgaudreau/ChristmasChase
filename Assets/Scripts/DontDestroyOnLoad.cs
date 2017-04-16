/*
* @author Gabriel Gaudreau - DontDestroyOnLoad.cs
* @date Jan. 2nd 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {
    
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
}
