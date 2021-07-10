using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerUI : MonoBehaviour
{
    public int selectedOption { get { return _option; } }


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

    public void SetOption(int option)
    {
        _option = option;
        if (IsAlive() && option != 0)
        {
            _optionSelectFX.Play();
        }
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
        if (_health.value <= 0f)
        {
            _health.value = 0f;
            UpdateImage();
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
}
