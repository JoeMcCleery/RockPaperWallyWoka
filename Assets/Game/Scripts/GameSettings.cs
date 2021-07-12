using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private NumberInputUI _healthInput;
    [SerializeField]
    private NumberInputUI _roundInput;
    [SerializeField]
    private NumberInputUI _revealInput;

    public void EnableSettings(bool enable)
    {
        this.gameObject.SetActive(enable);
    }

    public void StartGame()
    {
        if(PlayerManager.instance.GetAlivePlayers().Count < 1) { return; }
        GameManager.instance.health = GetHealth();
        GameManager.instance.roundDuration = GetRoundDuration();
        GameManager.instance.roundRevealDuration = GetRevealDuration();
        GameManager.instance.StartGame();
        EnableSettings(false);
    }

    public int GetHealth()
    {
        return (int)_healthInput.value;
    }

    public float GetRoundDuration()
    {
        return _roundInput.value;
    }

    public float GetRevealDuration()
    {
        return _revealInput.value;
    }
}
