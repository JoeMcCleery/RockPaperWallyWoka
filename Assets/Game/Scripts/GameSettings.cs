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
    [SerializeField]
    private NumberInputUI _wallyWokkaInput;
    [SerializeField]
    private NumberInputUI _wallyWokkaRoundInput;
    [SerializeField]
    private Toggle _damageIndicatorInput;

    public void EnableSettings(bool enable)
    {
        this.gameObject.SetActive(enable);
    }

    public void StartGame()
    {
        if(PlayerManager.instance.GetAlivePlayers().Count < 1) { return; }
        // Settings
        GameManager.instance.health = GetHealth();
        GameManager.instance.roundDuration = GetRoundDuration();
        GameManager.instance.roundRevealDuration = GetRevealDuration();
        GameManager.instance.showRoundDamage = GetShowDamageIndicator();
        GameManager.instance.wallyWokkaTotal = GetWallyWokkaTotal();
        GameManager.instance.wallyWokkaPerRound = GetWallyWokkaPerRound();

        // Update UI
        PlayerManager.instance.EnableWallyWokkaUI(GetWallyWokkaTotal() > 0 && GetWallyWokkaPerRound() != 0);

        // Start Game
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

    public bool GetShowDamageIndicator()
    {
        return _damageIndicatorInput.isOn;
    }

    public int GetWallyWokkaTotal()
    {
        return (int)_wallyWokkaInput.value;
    }

    public int GetWallyWokkaPerRound()
    {
        return (int)_wallyWokkaRoundInput.value;
    }
}
