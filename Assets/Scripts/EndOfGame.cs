using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndOfGame : MonoBehaviour {
    [SerializeField]
    Text _scoreText, _roundsText, _secondsText, _trapsText, _presentsText;

	void Start () {
        GameManager._gm.HideUI();
        _scoreText.text = "Score: " + Mathf.Round(GameManager._gm.GetScore());
        _roundsText.text = "Rounds Survived: " + (GameManager._gm.GetCurrentRound() - 1);
        _secondsText.text = "Seconds Survived: " + Mathf.Round(GameManager._gm.GetSeconds());
        _trapsText.text = "Traps Activated: " + GameManager._gm.GetTrapsActivated();
        _presentsText.text = "Presents Saved: " + GameManager._gm.GetPresentsSaved();
	}
}
