using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberInputUI : MonoBehaviour
{
    public float value;
    [SerializeField]
    private bool _isInteger;
    [SerializeField]
    private TextMeshProUGUI _valueText;
    [SerializeField]
    private float _incrementValue;
    [SerializeField]
    private float _minValue;
    [SerializeField]
    private float _maxValue;

    private void Start()
    {
        UpdateText();
    }

    public void IncrementUp()
    {
        value += _incrementValue;
        if (value > _maxValue)
        {
            value = _maxValue;
        }
        UpdateText();
    }

    public void IncrementDown()
    {
        value -= _incrementValue;
        if (value < _minValue)
        {
            value = _minValue;
        }
        UpdateText();
    }

    private void UpdateText()
    {
        _valueText.text = _isInteger ? ((int)value).ToString() : value.ToString("F2");
    }
}
