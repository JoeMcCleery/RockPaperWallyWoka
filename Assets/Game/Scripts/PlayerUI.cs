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

    public void OnRock(InputValue value)
    {
        if(value.isPressed)
        {
            SetOption(1);
        }
    }

    public void OnPaper(InputValue value)
    {
        if (value.isPressed)
        {
            SetOption(2);
        }
    }

    public void OnScissors(InputValue value)
    {
        if (value.isPressed)
        {
            SetOption(3);
        }
    }

    public void OnUse(InputValue value)
    {
        if (value.isPressed && IsAlive())
        {
            SetOption(-1); // Play selct option fx without changing selected option
        }
    }

    public void SetOption(int option)
    {
        if (GameManager.instance.PlayingRound() && IsAlive() && option != 0 && option != _option)
        {
            _optionSelectFX.Play();
            StartCoroutine(PlayerRumble(0.2f, 0.95f, 0.05f));
        }

        _option = option >= 0 ? option : _option;
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
        _health.value = 0.999999f; // Avoid rounding errors preventing player health to go to 0 on the last hit
        SetOption(0);
        UpdateImage();
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
}
