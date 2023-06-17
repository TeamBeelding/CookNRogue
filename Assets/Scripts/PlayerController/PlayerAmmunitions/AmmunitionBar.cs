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
        _ammoBar.maxValue = _playerAttack._ammunition;
        _ammoBar.value = _playerAttack._ammunition;
    }
    public void ResetAmmoBar()
    {
        _ammoBar.maxValue = 0;
        _ammoBar.value = 0;
    }
    public void UpdateAmmoBar()
    {
        _ammoBar.value = _playerAttack._ammunition;
        UpdateAmmoText();
    }
    public void AddIngredientAmmo(int nbr)
    {
        _ammoBar.maxValue += nbr;
        _ammoBar.value += nbr;
        UpdateAmmoText();
    }
    public void UpdateAmmoText()
    {
        if (_ammoText)
            _ammoText.text = _ammoBar.value.ToString();
    }
}
