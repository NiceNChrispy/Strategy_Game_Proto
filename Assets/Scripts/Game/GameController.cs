using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public enum GameState {Menu, Game, Paused, Builder }
    public GameState _gameState;
    public UIController uiController;

    [Header("Timer")]
    public float timerMax;
    public float currentTime;
    float timePercent;

    public enum PlayerTurn { Player1, Player2 }
    public PlayerTurn _playerTurn;

    // Use this for initialization
    void Start () {
        _gameState = GameState.Menu;
        StopAllCoroutines();
        StartCoroutine(TurnCountdown());
	}
	
	// Update is called once per frame
	void Update ()
    {
        timePercent = currentTime / timerMax;
	}

    public IEnumerator TurnCountdown()
    {
        timePercent = 1;
        currentTime = timerMax;
        uiController.timerFill.fillAmount = 1;
        int countdown = 0;
        while (countdown != -1)
        {
            if (currentTime > 0)
            {
                currentTime -= 1;
                uiController.timerFill.fillAmount = Mathf.Lerp(0, 1, timePercent);
                uiController.timer.text = currentTime.ToString();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                countdown = -1;
                EndTurn();
            }
        }
    }

    public void EndTurn()
    {
        StopAllCoroutines();
        StartCoroutine(TurnCountdown());
        SwitchPlayerTurn();
    }

    public void SwitchPlayerTurn()
    {
        if (_playerTurn == PlayerTurn.Player1)
        {
            _playerTurn = PlayerTurn.Player2;
        }
        else
        {
            _playerTurn = PlayerTurn.Player1;
        }
    }

    public void SetState(int state)
    {
        switch (state)
        {
            case 0:
                _gameState = GameState.Menu;
                break;

            case 1:
                _gameState = GameState.Game;
                break;

            case 2:
                _gameState = GameState.Paused;
                break;

            case 3:
                _gameState = GameState.Builder;
                break;

            default:
                _gameState = GameState.Menu;
                break;
        }
    }
}
