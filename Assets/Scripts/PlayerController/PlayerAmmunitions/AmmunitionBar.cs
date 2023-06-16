using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmunitionBar : MonoBehaviour
{
    [SerializeField]
    private Image m_ammoBar;
    private Material _ammoMat;
    private float _maxAmmo;
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
        _ammoMat = m_ammoBar.material;
        _ammoMat.SetFloat("_FillAmount", 0f);
    }

    public void InitAmmoBar(int ammoNbr)
    {
        _maxAmmo = _playerAttack._ammunition;
        _ammoMat.SetFloat("_FillAmount", 1f);
    }

    public void ResetAmmoBar()
    {
        _maxAmmo = 0f;
        _ammoMat.SetFloat("_FillAmount", 0f);
    }

    public void UpdateAmmoBar()
    {
        _ammoMat.SetFloat("_FillAmount", _playerAttack._ammunition / _maxAmmo);
        UpdateAmmoText();
    }
    public void AddIngredientAmmo(int nbr)
    {
        _maxAmmo += nbr;
        _ammoMat.SetFloat("_FillAmount", _playerAttack._ammunition);
        UpdateAmmoText();
    }

    public void UpdateAmmoText()
    {
        if (_ammoText)
            _ammoText.text = _playerAttack._ammunition.ToString();
    }
}
