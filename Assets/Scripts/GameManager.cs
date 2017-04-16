/*
* @author Gabriel Gaudreau - GameManager.cs
* @date Jan. 2nd 2017
* All copyrights go to Gabriel Gaudreau.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager _gm;

    [SerializeField]
    Texture2D _cursor;
    [SerializeField]
    Text _roundTimerText, _scoreText, _enemyCountText, _highScoreText, _roundNumberText, _roundNotifText;
    [SerializeField]
    private bool _isMenu;
    [SerializeField]
    Canvas _ui, _pauseMenu;
    [SerializeField]
    GameObject[] _enemySpawns, _presents, _presentSpawns; // Red = 0, Blue = 1, Gold = 2, Green = 3, White = 4;
    [SerializeField]
    GameObject _enemyPrefab, _player, _deadEnemy, _spawnPoint;

    private int _currentRound, _presentsSaved, _trapsActivated;
    private float _roundTimer, _score, _spawnTimer, _highScore, _seconds, _presentsTimer;
    private bool _changingRound, _isPaused, _playerAlive;

    /**
    * Start function. This function will check if Singleton _gm is null, if it is,
    * this function will assign the value of this instance of the script to it.
    * This function will find and properly display the current high score to the canvas item.
    */
    void Start() {
        _highScore = PlayerPrefs.GetFloat("High Score", _highScore);
        if (_gm == null) {
            _gm = this;
        }
        _highScoreText.text = "High Score: " + Mathf.Round(_highScore);
        Cursor.SetCursor(_cursor, new Vector2(_cursor.width / 2, _cursor.height / 2), CursorMode.Auto);
        _pauseMenu.enabled = false;
    }

    /**
    * This function will return the bool _changingRound
    * @return A boolean named _changingRound
    */
    public bool GetChangingRounds() {
        return _changingRound;
    }

    /**
    * This function will return the number of seconds since the start of the game.
    * @return This function will return a float value.
    */
    public float GetSeconds() {
        return _seconds;
    }

    /**
    * This function will return the current round.
    * @return This function will return an int value.
    */
    public int GetCurrentRound() {
        return _currentRound;
    }

    /**
    * This function will return the number of traps activated.
    * @return This function will return an int value.
    */
    public int GetTrapsActivated() {
        return _trapsActivated;
    }

    /**
    * This function will return the number of presents saved.
    * @return This function will return an int value.
    */
    public int GetPresentsSaved() {
        return _presentsSaved;
    }

    /**
    * This function will return the score of the player.
    * @return This function will return a float value.
    */
    public float GetScore() {
        return _score;
    }

    /**
    * This function will hide the UI Canvas.
    */
    public void HideUI() {
        _ui.enabled = false;
    }

    /**
    * This function will check the playerprefs files to compare current score vs highscore
    * if score is greater, a new highscore will be set in the file.
    */
    public void HighScoreCheck() {
        if (_score > _highScore) {
            PlayerPrefs.SetFloat("High Score", _score);
            _highScore = _score;
            _highScoreText.text = "High Score: " + Mathf.Round(_highScore);
        }
    }

    /**
    * This function will set the value of _trapsActivated.
    * @param This function takes in an int paramter.
    */
    public void SetNumTraps(int i) {
        _trapsActivated = i;
    }

    /**
    * This function will set the score of the player to a passed in value.
    * @param This function takes in a float parameter.
    */
    public void SetScore(float s) {
        _score = s;
    }

    /**
    * This function will set the value of _presentsSaved.
    * @param This function takes in an int paramter.
    */
    public void SetPresentsSaved(int i) {
        _presentsSaved = i;
    }

    /**
    * This function will set the boolean value of the playerAlive bool variable.
    */
    public void SetPlayerAlive(bool b) {
        _playerAlive = b;
    }

    /**
    * This function will return the value of the _playerAlive bool.
    * @return This function will return a type bool.
    */
    public bool GetPlayerAlive() {
        return _playerAlive;
    }

    /*
    * This function is public and will be called from the main menu.
    * This function will start the game by initializing needed variables.
    */
    public void StartGame() {
        _isMenu = false;
        _playerAlive = true;
        _currentRound = 1;
        _roundTimer = 20.0f + (_currentRound * 10.0f);
        _player.SetActive(true);
        _spawnTimer = 15.0f;
        _presentsTimer = 10.0f;
        _roundNumberText.text = "Round: " + _currentRound;
    }

    /**
    * This function will clear all enemies at the end of a round.
    */
    void ClearRound() {
        foreach (Enemy e in FindObjectsOfType<Enemy>()) {
            Instantiate(_deadEnemy, e.GetComponent<Transform>().position, Quaternion.identity);
            Destroy(e.gameObject);
        }
    }


    /*
    * This function will summon waves of enemies every 15 seconds. (5 new enemies)
    * This function will also initiate presents are precalculated locations. (3 presents)
    */
    void SpawnWave() {
        foreach (GameObject s in _enemySpawns) {
            Instantiate(_enemyPrefab, s.transform.position, Quaternion.identity);
        }
        _spawnTimer = 15.0f;
    }

    /**
    * This function will determine what color present is to be instantiated.
    * @param This function takes in an integer.
    * @return This function will return the type of present, as an integer.
    */
    int DetermineType(int i) {
        int type = -1;
        if (0 <= i && i <= 10) { // Red - Kills enemies.
            type = 0;
        }
        else if (11 <= i && i <= 30) { // Gold - Score x2.
            type = 2;
        }
        else if (31 <= i && i <= 60) { // Green - Invincible.
            type = 3;
        }
        else if (61 <= i && i <= 90) { // Blue - Speed.
            type = 1;
        }
        else { // White - Full life.
            type = 4;
        }
        return type;
    }

    /**
    * This function will generate 4 indices to use for spawning locations for the present.
    * This function will also generate 4 types of presents that will be spawned.
    */
    void SpawnPresents() {
        int[] _spawns = { 0, 0, 0, 0 };
        _spawns[0] = Random.Range(0, 13);
        int mod = Random.Range(1, 12);
        while (mod == 6) { // mod == 6 would mean having more than one presents spawn at the same location.
            mod = Random.Range(1, 12);
        }
        _spawns[1] = (_spawns[0] + mod) % 12;
        _spawns[2] = (_spawns[1] + mod) % 12;
        _spawns[3] = (_spawns[2] + mod) % 12;
        for (int i = 0; i < 4; ++i) {
            int rand = Random.Range(0, 100);
            int type = DetermineType(rand);
            Instantiate(_presents[type], _presentSpawns[_spawns[i]].transform.position, Quaternion.identity);
        }
        _presentsTimer = 10.0f;
    }
    


    /**
    * This function will reload the scene into the next round.
    * This function will reset timers and update canvas items to display proper values.
    * @return This function will return a IEnumerator in the form of a WaitForSeconds call.
    */
    IEnumerator NextRound() {
        _changingRound = true;
        _player.GetComponent<Player>().SetInvincible(true);
        ClearRound();
        HighScoreCheck();
        _roundNotifText.text = "Round " + _currentRound + " Survived!";
        yield return new WaitForSeconds(1.0f);
        _currentRound++;
        _roundNumberText.text = "Round: " + _currentRound;
        _player.GetComponent<Player>().SetInvincible(false);
        _roundNotifText.text = "Round " + _currentRound + " in 3";
        yield return new WaitForSeconds(1.0f);
        _roundNotifText.text = "Round " + _currentRound + " in 2";
        yield return new WaitForSeconds(1.0f);
        _roundNotifText.text = "Round " + _currentRound + " in 1";
        yield return new WaitForSeconds(1.0f);
        _roundNotifText.text = "GO!";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _changingRound = false;
        _roundTimer = 20.0f + (_currentRound * 10.0f);
        _spawnTimer = 15.0f;
        _presentsTimer = 10.0f;
        yield return new WaitForSeconds(1.0f);
        _roundNotifText.text = "";
    }

    /*
    * Update function. This executes every frame.
    * This function controls the game.
    * This function handles round timers, spawn timers and player score.
    * This function handles input for pausing the game.
    */
    void Update() {
        if (!_isMenu && _playerAlive) {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused) {
                Pause();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && _isPaused) {
                Unpause();
            }
            if (_roundTimer > 0) {
                _roundTimer -= Time.deltaTime;
                _seconds += Time.deltaTime;
                _spawnTimer -= Time.deltaTime;
                _presentsTimer -= Time.deltaTime;
                _roundTimerText.text = "Time: " + Mathf.Round(_roundTimer);
                _score += Time.deltaTime * 100;
                _scoreText.text = "Score: " + Mathf.Round(_score);
                if (_spawnTimer < 0) {
                    SpawnWave();
                }
                if(_presentsTimer < 0) {
                    SpawnPresents();
                }
            }
            else {
                if (!_changingRound) {
                    StartCoroutine(NextRound());
                }
            }
        }
    }

    /**
    * This function will assign the boolean value of _isPaused to its opposite
    */
    void TogglePause() {
        _isPaused = !_isPaused;
    }

    /**
    * This function will Unpause the game.
    * This function will hide the pause canvas and display the UI canvas.
    * Sets time scale to 1.
    */
    void Unpause() {
        Time.timeScale = 1;
        TogglePause();
        _ui.enabled = true;
        _pauseMenu.enabled = false;
    }

    /**
    * This function will pause the game.
    * This function will disable to UI and display the pause canvas.
    * Sets time scale to 0.
    */
    void Pause() {
        Time.timeScale = 0;
        TogglePause();
        _ui.enabled = false;
        _pauseMenu.enabled = true;
    }

    /**
    * This function will be called when clicking the resume button on the pause canvas.
    */
    public void OnClickResume() {
        Unpause();
    }

    /**
    * This function will be called when clicking on the back to main menu button on the pause canvas.
    * This function will explicitely destroy all objects that are set to DontDestroyOnLoad.
    * Sets time scale back to 1.
    */
    public void OnClickBackToMainMenu() {
        foreach (DontDestroyOnLoad o in FindObjectsOfType<DontDestroyOnLoad>()) {
            Destroy(o.gameObject);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    /**
    * This function will exit the game when called. Will automatically save score if applicable.
    */
    public void OnClickQuitGame() {
        HighScoreCheck(); // Save & Quit.
        Application.Quit();
    }
}

//TODO:
//change character color for present
//sound.
//testing.
//clean up unneeded files.

//BUGS:
//presents executing twice sometimes: -red, -gold.

//TUNING LOG.
//more presents, more often (timer: 10s from 15s, #presents: 4 from 3).

