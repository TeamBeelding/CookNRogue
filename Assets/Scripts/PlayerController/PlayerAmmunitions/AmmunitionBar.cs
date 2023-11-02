using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AmmunitionBar : MonoBehaviour
{
    private Slider _ammoBar;
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
        _ammoBar.maxValue = PlayerRuntimeData.GetInstance().data.AttackData.Ammunition;
        _ammoBar.value = PlayerRuntimeData.GetInstance().data.AttackData.Ammunition;
    }
    public void ResetAmmoBar()
    {
        _ammoBar.maxValue = 0;
        _ammoBar.value = 0;
    }
    public void UpdateAmmoBar()
    {
        _ammoBar.value = PlayerRuntimeData.GetInstance().data.AttackData.Ammunition;
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
