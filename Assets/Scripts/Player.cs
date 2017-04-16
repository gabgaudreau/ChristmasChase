/*
* @author Gabriel Gaudreau - Player.cs
* @date Jan. 1st 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [SerializeField]
    Text _hpText, _presentNotifText;
    [SerializeField]
    GameObject _deadPlayer, _deadEnemy;

    private NavMeshAgent _navAgent;
    private bool _collidingWithController, _invincible, _red, _gold, _green, _blue, _white, _activated, _playerIsDead;
    private GameObject _tempTrap;
    private GameObject _interactNotif;
    private Text _interactNotifText;
    private int _hp, _presentsSaved, _trapsActivated;
    private MeshRenderer[] _meshRends;

    /**
    * Start function. This function will find the "Interact_Notif" GameObject from the UI canvas.
    * In order to display notifications to the player.
    */
    void Start() {
        _interactNotif = GameObject.Find("Interact_Notif");
        if (_interactNotif == null) {
            Debug.Log("Error finding Interact_Notif. Exiting");
            Application.Quit();
        }
        _interactNotifText = _interactNotif.GetComponent<Text>();
        _navAgent = GetComponent<NavMeshAgent>();
        _hp = 4;
        UpdateLifeText();
        _meshRends = GetComponentsInChildren<MeshRenderer>();
    }

    /**
    * This function will set the invincible bool of the player.
    */
    public void SetInvincible(bool b) {
        _invincible = b;
    }

    /**
    * This function will update the health text object on the canvas.
    */
    void UpdateLifeText() {
        _hpText.text = "Health: " + _hp;
    }

    /**
    * This function is a public version of the NavAgent's SetDestination.
    * This function will set the destination of the NavAgent to the passed in location.
    * @param A vector3 location is passed in as a parameter.
    */
    public void SetDestination(Vector3 location) {
        _navAgent.SetDestination(location);
    }

    /**
    * Update function. This function will execute once per frame.
    * This function handles input from the user for interacting with the trap controllers.
    */
    void Update() {
        if (_collidingWithController && (Input.GetButton("Interact"))) {
            _tempTrap.GetComponent<TrapController>().Activate();
            _activated = true;
        }
    }

    /**
    * This function will hide the mesh of the player.
    */
    void HideMesh() {
        foreach (MeshRenderer m in _meshRends) {
            m.GetComponent<Renderer>().enabled = false;
        }
    }

    /**
    * This function will show the mesh of the player.
    */
    void ShowMesh() {
        foreach (MeshRenderer m in _meshRends) {
            m.GetComponent<Renderer>().enabled = true;
        }
    }

    /**
    * This function will handle deducting life from the player when he is hit by an enemy.
    * This function will alternatively hide and show the player mesh, giving the visual effect of getting hit
    * and being invincible for a few seconds after taking damage.
    * @return This function will return a type IEnumerator in the form of a WaitForSeconds call. (multiple calls).
    */
    IEnumerator DeductLife() {
        _invincible = true;
        _hp--;
        UpdateLifeText();
        if (_hp == 0) {
            StartCoroutine(Death());
        }
        else {
            for (int i = 0; i < 3; ++i) {
                HideMesh();
                yield return new WaitForSeconds(0.15f);
                ShowMesh();
                yield return new WaitForSeconds(0.35f);

            }
            yield return new WaitForSeconds(0.45f);
            _invincible = false;
        }
    }

    /**
    * This function will be executed once the player reaches 0 hp.
    * This function will handle the visual effect of dying as well as the scene management aspect.
    * @return This function will return a type IEnumerator in the form of a WaitForSeconds call.
    */
    IEnumerator Death() {
        _playerIsDead = true;
        GameManager._gm.SetPlayerAlive(false); // Player is dead.
        GameManager._gm.HighScoreCheck(); // Update High Score if applicable.
        GameManager._gm.SetNumTraps(_trapsActivated);
        GameManager._gm.SetPresentsSaved(_presentsSaved);
        Instantiate(_deadPlayer, _navAgent.transform.position, Quaternion.identity);
        HideMesh();
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(3); // End Scene.
    }

    /**
    * This function will execute when the player collides with a blue present.
    * @return This function will return a type IEnumerator in the form of a waitforseconds call.
    */
    IEnumerator BluePresent() {
        _navAgent.speed = 12.5f;
        _presentNotifText.text = "Speed Boost - 3s";
        yield return new WaitForSeconds(3.0f);
        _presentNotifText.text = "";
        _navAgent.speed = 8.5f;
    }

    /**
    * This function will execute when the player collides with a green present.
    * @return This function will return a type IEnumerator in the form of a waitforseconds call.
    */
    IEnumerator GreenPresent() {
        _invincible = true;
        _presentNotifText.text = "Invincibility - 3s";
        yield return new WaitForSeconds(3.0f);
        _presentNotifText.text = "";
        _invincible = false;
    }

    /**
    * 
    */
    IEnumerator PresentNotif(int i) {
        if(i == 0) { //Red
            _presentNotifText.text = "Boom goes the dynamite!";
            yield return new WaitForSeconds(1.0f);
            _presentNotifText.text = "";
        }
        else if(i == 1) { // White
            _presentNotifText.text = "Full health!";
            yield return new WaitForSeconds(1.0f);
            _presentNotifText.text = "";
        }
        else if(i == 2) { //Gold
            _presentNotifText.text = "Score Bonus!";
            yield return new WaitForSeconds(1.0f);
            _presentNotifText.text = "";
        }
    }

    /**
    * This function will be executed when the player collides with a red present.
    * This function will find all enemies and instantiate at their position the dead enemy prefab 
    * followed by destroying the actual enemy.
    */
    void RedPresent() {
        // Nuke sound. (explosion, emp, something)
        foreach (Enemy e in FindObjectsOfType<Enemy>()) {
            Instantiate(_deadEnemy, e.GetComponent<Transform>().position, Quaternion.identity);
            Destroy(e.gameObject);
        }
        _red = false;
    }

    /**
    * This function handles collision event between the player and the enemies.
    * This function will also handle all collision between the player and all the different color presents.
    * @param colliding object is passed in as a parameter.
    */
    void OnCollisionEnter(Collision col) {
        if (GameManager._gm.GetPlayerAlive()) {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                if (!_invincible) {
                    StartCoroutine(DeductLife());
                }
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("Present")) {
                if (col.gameObject.tag == "RedPresent") { // Kills all enemies.
                    if (!_red) { // Needs further testing. - summons more than one deadEnemy prefab.
                        _red = true;
                        StartCoroutine(PresentNotif(0));
                        Invoke("RedPresent", 1.0f);
                        _presentsSaved++;
                        Destroy(col.gameObject);
                    }
                }
                else if (col.gameObject.tag == "GoldPresent") { // Current score x2
                    col.gameObject.GetComponent<MeshRenderer>().enabled = false; // Destroyed on exit.
                }
                else if (col.gameObject.tag == "GreenPresent") { // Invincible - change material color 
                    StartCoroutine(GreenPresent());
                    _presentsSaved++;
                    Destroy(col.gameObject);
                }
                else if (col.gameObject.tag == "BluePresent") { // Speed boost - change material color
                    StartCoroutine(BluePresent());
                    _presentsSaved++;
                    Destroy(col.gameObject);
                }
                else if (col.gameObject.tag == "WhitePresent") { // Full life
                    _hp = 4;
                    StartCoroutine(PresentNotif(1));
                    UpdateLifeText();
                    _presentsSaved++;
                    Destroy(col.gameObject);
                }
            }
        }
    }

    /**
    * This function handles when the player exits another object's collider box.
    * @param This function takes in a collision object as a parameter.
    */
    void OnCollisionExit(Collision col) {
        if (GameManager._gm.GetPlayerAlive()) {
            if (col.gameObject.layer == LayerMask.NameToLayer("Present")) {
                if (col.gameObject.tag == "GoldPresent") {
                    if (!_gold) { // Needs more testing. - executes twice, score becomes way too large.
                        _gold = true;
                        GameManager._gm.SetScore(GameManager._gm.GetScore() * 1.2f);
                        _gold = false;
                        StartCoroutine(PresentNotif(2));
                        _presentsSaved++;
                        Destroy(col.gameObject);
                    }
                }
            }
        }
    }


    /**
    * This function handles when the player enters a trigger zone.
    * @param This function takes in a Collider object.
    */
    void OnTriggerEnter(Collider col) {
        if (GameManager._gm.GetPlayerAlive()) {
            if (col.gameObject.layer == LayerMask.NameToLayer("TrapController")) {
                if (!col.gameObject.GetComponent<TrapController>().GetMoving()) {
                    _interactNotifText.text = "\"E\" to interact !";
                }
                else {
                    _interactNotifText.text = "";
                }
                _collidingWithController = true;
                _tempTrap = col.gameObject;
            }
        }
    }

    /**
    * This function handles when the player stay within a trigger zone.
    * @param This function takes in a Collider object.
    */
    void OnTriggerStay(Collider col) {
        if (GameManager._gm.GetPlayerAlive()) {
            if (col.gameObject.layer == LayerMask.NameToLayer("TrapController")) {
                if (!col.gameObject.GetComponent<TrapController>().GetMoving()) {
                    _interactNotifText.text = "\"E\" to interact !";
                }
                else {
                    _interactNotifText.text = "";
                }
            }
        }
    }

    /**
    * This function handles when the player stay within a trigger zone.
    * @param This function takes in a Collider object.
    */
    void OnTriggerExit(Collider col) {
        if (GameManager._gm.GetPlayerAlive()) {
            if (col.gameObject.layer == LayerMask.NameToLayer("TrapController")) {
                if (_activated) {
                    _trapsActivated++;
                    _activated = false;
                }
                _interactNotifText.text = "";
                _collidingWithController = false;
                _tempTrap = null;
            }
        }
    }
}