using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public int wallyWokkaTotal;
    [HideInInspector]
    public int wallyWokkaPerRound;
    [HideInInspector]
    public float roundDuration;
    [HideInInspector]
    public float roundRevealDuration;
    [HideInInspector]
    public bool showRoundDamage;

    [SerializeField]
    private float _roundDelay;
    private double _roundStartTime;
    private float _totalRoundDuration;
    private double _roundEndTime;
    private int _currentRound = 0;
    [SerializeField]
    private Slider _roundTimer;
    [SerializeField]
    private Slider _roundRevealTimer;
    [SerializeField]
    private GameSettings _gameSettings;

    private bool _playRound;
    private bool _playRevealRound;

    [SerializeField]
    private TextMeshProUGUI _countDownText;

    private void Start()
    {
        if (GameManager.instance != null) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        health = _gameSettings.GetHealth();
    }

    void Update()
    {
        if (_playRound)
        {
            var val = Time.deltaTime / roundDuration;
            val += _roundTimer.value;
            if (val >= 1f)
            {
                RoundTimerEnded();
                val = 1f;
            }
            _roundTimer.value = val;
        }
        else if (_playRevealRound)
        {
            var val = Time.deltaTime / roundRevealDuration;
            val += _roundRevealTimer.value;
            if (val >= 1f)
            {
                RoundRevealTimerEnded();
                val = 1f;
            }
            _roundRevealTimer.value = val;
            PlayerManager.instance.UpdatePlayerImages();
            var Damages = GetAllRoundDamages();
            for (int i = 0; i < Damages.Length; i++)
            {
                PlayerManager.instance.Players[i].ui.UpdateDamageUI(showRoundDamage ? Damages[i] : 0);
            }
        }
        if(PlayingRound())
        {
            _countDownText.text = ((float)((_roundEndTime - Time.realtimeSinceStartupAsDouble) / _totalRoundDuration * 3f)).ToString("F1");
        }
    }

    public bool PlayingRound()
    {
        return _playRound || _playRevealRound;
    }

    public bool PlayingRevealRound()
    {
        return _playRevealRound;
    }

    private void RoundTimerEnded()
    {
        PlayerManager.instance.SetDefaultOption(); // Set a default option if none selected
        _playRound = false;
        _playRevealRound = true;
        PlayerManager.instance.UpdatePlayerImages();
    }

    private void RoundRevealTimerEnded()
    {
        Debug.Log("Round " + _currentRound.ToString() + " has ended.");
        _playRound = false;
        _playRevealRound = false;
        _countDownText.text = "";
        PlayerManager.instance.UpdatePlayerImages();
        var Damages = GetAllRoundDamages();
        for (int i = 0; i < Damages.Length; i++)
        {
            PlayerManager.instance.Players[i].ui.LoseHealth(Damages[i]);
        }
        var alivePlayers = PlayerManager.instance.GetAlivePlayers();
        Debug.Log(alivePlayers.Count);
        if (alivePlayers.Count <= 1)
        {
            PlayerManager.instance.SetRoundWinner(alivePlayers.Count == 1 ? alivePlayers[0].ui.GetPlayerID() : -1);
            ResetGame();
            return;
        }

        StartRoundAfterDelay();
    }

    public int[] GetAllRoundDamages()
    {
        var Players = PlayerManager.instance.Players;
        int[] Damages = new int[Players.Count];
        for (int i = 0; i < Players.Count; i++)
        {
            Damages[i] = CalculateRoundLosses(Players[i].ui.selectedOption, Players[i].ui.GetPlayerID());
        }
        return Damages;
    }

    public int CalculateRoundLosses(int option, int playerID)
    {
        var Players = PlayerManager.instance.Players;
        int DamageTaken = 0;
        for (int j = 0; j < Players.Count; j++)
        {
            if(Players[j].ui.GetPlayerID() == playerID) { continue; }

            switch (option)
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
                            DamageTaken++;
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
                            DamageTaken++;
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
                            DamageTaken++;
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
       
        return DamageTaken;
    }

    public int CalculateRoundWins(int option, int playerID)
    {
        var Players = PlayerManager.instance.Players;
        int DamageDealt = 0;
        for (int j = 0; j < Players.Count; j++)
        {
            if (Players[j].ui.GetPlayerID() == playerID) { continue; }

            switch (option)
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
                            break;
                        case 3: // Scissors
                                // Win
                            DamageDealt++;
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
                            DamageDealt++;
                            break;
                        case 2: // Paper
                                // Draw
                            break;
                        case 3: // Scissors
                                // Lose
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
                            break;
                        case 2: // Paper
                                // Win
                            DamageDealt++;
                            break;
                        case 3: // Scissors
                                // Draw
                            break;
                    }
                    break;
            }
        }

        return DamageDealt;
    }

    public void ResetGame()
    {
        this.StopAllCoroutines();

        PlayerManager.instance.ResetPlayers();
        _playRound = false;
        _playRevealRound = false;
        _countDownText.text = "";
        _roundTimer.value = 0f;
        _roundRevealTimer.value = 0f;
        _currentRound = 0;

        ShowSettings();
    }

    public void ShowSettings()
    {
        _gameSettings.EnableSettings(true);
    }

    public void StartGame()
    {
        StartRoundAfterDelay();
    }

    private void StartRoundAfterDelay()
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
        PlayerManager.instance.PlayerRoundReset();
        _playRound = true;
        _playRevealRound = false;
        _roundStartTime = Time.realtimeSinceStartupAsDouble;
        _totalRoundDuration = roundDuration + roundRevealDuration;
        _roundEndTime = _roundStartTime + _totalRoundDuration;
        _roundTimer.value = 0f;
        _roundRevealTimer.value = 0f;
        _currentRound++;
        Debug.Log("Round " + _currentRound.ToString() + " has started.");
    }
}
