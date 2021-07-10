using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GameSettings : MonoBehaviour
{
    public int rounds = 3;
    [SerializeField]
    private TextMeshProUGUI _roundsText;

    public void EnableSettings(bool enable)
    {
        this.gameObject.SetActive(enable);
    }

    public void StartGame()
    {
        if(PlayerManager.instance.GetAlivePlayers().Count < 1) { return; }
        GameManager.instance.numRounds = rounds;
        GameManager.instance.StartGame();
        EnableSettings(false);
    }

    public void AddRound()
    {
        rounds++;
        UpdateText();
    }

    public void RemoveRound()
    {
        rounds--;
        if(rounds < 1)
        {
            rounds = 1;
        }
        UpdateText();
    }

    private void UpdateText()
    {
        _roundsText.text = rounds.ToString();
    }
}
