using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    public int selectedOption { get { return _option; } }

    [SerializeField]
    private Sprite[] _sprites;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private int _option;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Slider _health;
    [SerializeField]
    private Slider _wallyWokkaMeter;
    [SerializeField]
    private GameObject _wallyWokkaGO;
    [SerializeField]
    private VisualEffect _damagedFX;
    [SerializeField]
    private VisualEffect _optionSelectFX;
    [SerializeField]
    private TextMeshProUGUI _playerIDText;
    [SerializeField]
    private RoundDamage _roundDamage;
    [SerializeField]
    private GameObject _crown;

    private int _playerID;
    private int _roundWallyWokkaCount;

    public void OnRock(InputValue value)
    {
        if(value.isPressed)
        {
            SetOption(1, 1);
        }
    }

    public void OnPaper(InputValue value)
    {
        if (value.isPressed)
        {
            SetOption(2, 1);
        }
    }

    public void OnScissors(InputValue value)
    {
        if (value.isPressed)
        {
            SetOption(3, 1);
        }
    }

    public void OnUse(InputValue value)
    {
        if (value.isPressed && IsAlive())
        {
            // Default to no change but play FX
            int bestOption = -1;

            // only allow auto wally wokka during the reveal phase
            if (GameManager.instance.PlayingRevealRound())
            {
                int mostDamage = 0;
                int tempDamageDealt = 0;
                for (int o = 1; o <= 3; o++)
                {
                    tempDamageDealt = GameManager.instance.CalculateRoundWins(o, GetPlayerID());

                    if (mostDamage < tempDamageDealt)
                    {
                        mostDamage = tempDamageDealt;
                        bestOption = o;
                    }
                }
            }

            SetOption(bestOption, 2);
        }
    }

    public void SetOption(int option, int cost = 0)
    {
        bool canChange = true;

        if (GameManager.instance.PlayingRound() && IsAlive() && option != 0 && option != _option)
        {
            if(GameManager.instance.PlayingRevealRound())
            {
                canChange = LoseWallyWokka(cost);
            }

            if(canChange)
            {
                _optionSelectFX.Play();
                StartCoroutine(PlayerRumble(0.2f, 0.95f, 0.05f));
            }
        }

        _option = (option >= 0 && canChange) ? option : _option;
    }

    public void UpdateImage()
    {
        if(_health.value <= 0f)
        {
            _option = 0;
        }
        _image.sprite = _sprites[_option];
    }
    
    public void LoseHealth(int damage = 1)
    {
        if(damage < 1) { return; }
        _health.value -= (float)damage / (float)GameManager.instance.health;
        _damagedFX.Play();
        StartCoroutine(PlayerRumble(0.85f, 0.95f, 0.3f));
        if (_health.value <= 0f)
        {
            _health.value = 0f;
            UpdateImage();
        }
    }

    public bool LoseWallyWokka(int ammount = 1)
    {
        if (ammount < 1 || (_roundWallyWokkaCount >= GameManager.instance.wallyWokkaPerRound && GameManager.instance.wallyWokkaPerRound >= 0) || (_wallyWokkaMeter.value < (float)ammount / (float)GameManager.instance.wallyWokkaTotal && GameManager.instance.wallyWokkaTotal >= 0)) { return false; }
        if(GameManager.instance.wallyWokkaTotal > 0)
        {
            _wallyWokkaMeter.value -= (float)ammount / (float)GameManager.instance.wallyWokkaTotal;
            if (_wallyWokkaMeter.value <= 0f)
            {
                _wallyWokkaMeter.value = 0f;
            }
        }
        _roundWallyWokkaCount++;
        return true;
    }

    public void UpdateDamageUI(int damage)
    {
        _roundDamage.SetDamage(damage);
    }

    private IEnumerator PlayerRumble(float low, float high, float duration)
    {
        var gamepad = PlayerGamepad();
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(low, high);
            yield return new WaitForSeconds(duration);
            gamepad.SetMotorSpeeds(0f, 0f);
            gamepad.ResetHaptics();
        }
    }

    public void ResetUI()
    {
        _health.value = 0.99999f;
        _wallyWokkaMeter.value = 1f;
        ResetRoundWallyWokka();
        SetOption(0);
        UpdateImage();
    }

    public void ResetRoundWallyWokka()
    {
        _roundWallyWokkaCount = 0;
    }

    public bool IsAlive()
    {
        return _health.value > 0f;
    }

    public void SetPlayerID(int id)
    {
        _playerID = id;
        _playerIDText.text = _playerID.ToString();
    }

    public int GetPlayerID()
    {
        return _playerID;
    }

    private Gamepad PlayerGamepad()
    {
        var gamepad = _playerInput.devices.ToArray()[0];
        var allInputs = Gamepad.all.ToArray();

        for (int i = 0; i < allInputs.Length; i++) {
            if(allInputs[i].Equals(gamepad))
            {
                return allInputs[i];
            }
        }

        return null;
    }

    public void ShowCrown(bool show)
    {
        _crown.SetActive(show);
    }

    public void ShowWallyWokka(bool show)
    {
        _wallyWokkaGO.SetActive(show);
    }
}
