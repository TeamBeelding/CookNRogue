using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmunitionBar : MonoBehaviour
{
    private Slider _ammoBar;
    public PlayerAttack _playerAttack;
    public TextMeshProUGUI _ammoText;
    public static AmmunitionBar instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    private void Start()
    {
        _ammoBar = GetComponentInChildren<Slider>();
    }
    public void InitAmmoBar(int ammoNbr)
    {
        Debug.Log("init ammo");
        _ammoBar.maxValue= _playerAttack._ammunition;
        _ammoBar.value= _playerAttack._ammunition;
    }

    public void UpdateAmmoBar()
    {
        Debug.Log("update ammo");
        _ammoBar.value = _playerAttack._ammunition;
        _ammoText.text = _ammoBar.value.ToString();
    }
    public void AddIngredientAmmo(int nbr)
    {
        _ammoBar.maxValue += nbr;
        _ammoBar.value += nbr;
        _ammoText.text = _ammoBar.value.ToString();
    }
}
