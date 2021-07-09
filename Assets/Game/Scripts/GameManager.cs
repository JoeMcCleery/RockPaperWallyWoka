using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int numRounds;
    private int _currentRound = 0;
    [SerializeField]
    private float _roundDuration;
    [SerializeField]
    private float _roundDelay;
    [SerializeField]
    private float _resetDelay;
    [SerializeField]
    private Slider _roundTimer;

    private bool _playRound;

    private void Start()
    {
        if (GameManager.instance != null) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    void Update()
    {
        if (_playRound)
        {
            var val = Time.deltaTime / _roundDuration;
            val += _roundTimer.value;
            if (val >= 1f)
            {
                RoundTimerEnded();
                val = 1f;
            }
            _roundTimer.value = val;
        }
    }

    private void RoundTimerEnded()
    {
        Debug.Log("Round " + _currentRound.ToString() + " has ended.");
        _playRound = false;
        PlayerManager.instance.UpdatePlayerImages();
        UpdateScores();
        var alivePlayers = PlayerManager.instance.GetAlivePlayers();
        Debug.Log(alivePlayers.Count);
        if (alivePlayers.Count <= 1)
        {
            ResetGame();
            return;
        }

        StartRoundAfterDelay();
    }

    public void UpdateScores()
    {
        var Players = PlayerManager.instance.Players;
        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < Players.Count; j++)
            {
                if (i == j) { continue; }
                switch (Players[i].ui.selectedOption)
                {
                    default:
                    case 0:
                        break;
                    case 1: // Rock
                        switch (Players[j].ui.selectedOption)
                        {
                            default:
                            case 0:
                                break;
                            case 1: // Rock
                                // Draw
                                break;
                            case 2: // Paper
                                // Lose
                                Players[i].ui.LoseHealth();
                                break;
                            case 3: // Scissors
                                // Win
                                break;
                        }
                        break;
                    case 2: // Paper
                        switch (Players[j].ui.selectedOption)
                        {
                            default:
                            case 0:
                                break;
                            case 1: // Rock
                                // Win
                                break;
                            case 2: // Paper
                                // Draw
                                break;
                            case 3: // Scissors
                                // Lose
                                Players[i].ui.LoseHealth();
                                break;
                        }
                        break;
                    case 3: // Scissors
                        switch (Players[j].ui.selectedOption)
                        {
                            default:
                            case 0:
                                break;
                            case 1: // Rock
                                // Lose
                                Players[i].ui.LoseHealth();
                                break;
                            case 2: // Paper
                                // Win
                                break;
                            case 3: // Scissors
                                // Draw
                                break;
                        }
                        break;
                }
            }
        }
    }

    public void ResetGame()
    {
        this.StopAllCoroutines();

        ResetGameAfterDelay();
    }

    public void ResetGameAfterDelay()
    {
        this.StopAllCoroutines();
        _playRound = false;
        StartCoroutine(ResetGameCoroutine());
    }

    private IEnumerator ResetGameCoroutine()
    {
        yield return new WaitForSeconds(_resetDelay);
        _roundTimer.value = 0f;
        _currentRound = 0;
        StartGame();
    }

    public void StartGame()
    {
        PlayerManager.instance.ResetPlayers();
        StartRoundAfterDelay();
    }

    public void StartRoundAfterDelay()
    {
        this.StopAllCoroutines();

        StartCoroutine(StartRoundCoroutine());
    }

    private IEnumerator StartRoundCoroutine()
    {
        yield return new WaitForSeconds(_roundDelay);
        StartRound();
    }

    private void StartRound()
    {
        PlayerManager.instance.ResetPlayerOptions();
        PlayerManager.instance.SeRandomPlayerOptionsWithoutShowing();
        _playRound = true;
        _roundTimer.value = 0f;
        _currentRound++;
        Debug.Log("Round " + _currentRound.ToString() + " has started.");
    }
}
