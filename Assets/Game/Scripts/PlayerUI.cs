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
    private TextMeshProUGUI _text;
    [SerializeField]
    private Sprite[] _sprites;

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
            _optionSelectFX.Play(); // Fake setting option
        }
    }

    public void SetOption(int option)
    {
        if (IsAlive() && option != 0 && option != _option)
        {
            _optionSelectFX.Play();
        }
        _option = option;
    }

    public void UpdateImage()
    {
        if(_health.value <= 0f)
        {
            _option = 0;
        }
        _image.sprite = _sprites[_option];
    }
    
    public void LoseHealth()
    {
        _health.value -= 1.001f / GameManager.instance.numRounds;
        _damagedFX.Play();
        StartCoroutine(PlayerRumble(0.5f, 0.3f, 0.2f));
        if (_health.value <= 0f)
        {
            _health.value = 0f;
            UpdateImage();
        }
    }

    private IEnumerator PlayerRumble(float low, float high, float duration)
    {
        var gamepad = PlayerGamepad();
        if (gamepad != null)
        {
            gamepad.ResumeHaptics();
            gamepad.SetMotorSpeeds(low, high);
            yield return new WaitForSeconds(duration);
            gamepad.SetMotorSpeeds(0f, 0f);
            gamepad.ResetHaptics();
        }
    }

    public void ResetUI()
    {
        _health.value = 1f;
        _option = 0;
        _image.sprite = _sprites[0];
    }

    public bool IsAlive()
    {
        return _health.value > 0f;
    }

    public void SetPlayerID(int id)
    {
        _text.text = id.ToString();
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
}
