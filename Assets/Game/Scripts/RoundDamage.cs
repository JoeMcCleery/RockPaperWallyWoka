using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundDamage : MonoBehaviour
{
    private int _damage = 0;
    [SerializeField]
    private TextMeshProUGUI _damageText;

    public void Start()
    {
        SetDamage(0);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
        _damageText.text = damage.ToString();

        if(damage == 0 && this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
        else if (damage > 0 && !this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(true);
        }
    }
}
