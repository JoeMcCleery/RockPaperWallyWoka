using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private int _option;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Slider _health;
    [SerializeField]
    private Sprite[] sprites;

    public void SetOption(int option)
    {
        _option = option;
        SetImage(option);
    }

    private void SetImage(int option)
    {
        _image.sprite = sprites[option];
    }

    public void SetHealth(float health = 1f)
    {
        _health.value = health;
    }
}
