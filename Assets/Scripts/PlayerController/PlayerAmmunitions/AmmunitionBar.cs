using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AmmunitionBar : MonoBehaviour
{
    [SerializeField]
    private Image _ammoBar;
    [SerializeField]
    private GameObject _gageObject;

    [Header("Lid")]
    [SerializeField]
    private Transform _cauldronLidTrans;
    [SerializeField]
    private Transform _lidOpenedPos;
    [SerializeField]
    private Transform _lidClosedPos;

    public static AmmunitionBar instance;

    private Material _barMaterial;
    private float _maxMunitions = 0f;

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
        Material mat = Instantiate(_ammoBar.material);
        _ammoBar.material = mat;
        _barMaterial = _ammoBar.material;
    }

    public void InitAmmoBar()
    {
        _maxMunitions = PlayerRuntimeData.GetInstance().data.AttackData.Ammunition;
        _barMaterial.SetFloat("_FillAmount", 1f);
        _barMaterial.SetFloat("_GradientValue", 0f);
        _gageObject.SetActive(true);
        _cauldronLidTrans.position = _lidOpenedPos.position;
    }
    public void ResetAmmoBar(bool playAnim)
    {
        _maxMunitions = 0f;
        _barMaterial.SetFloat("_FillAmount", 0f);
        _barMaterial.SetFloat("_GradientValue", 1f);
        _gageObject.SetActive(false);
        _cauldronLidTrans.position = _lidClosedPos.position;
    }
    public void UpdateAmmoBar()
    {
        _barMaterial.SetFloat("_FillAmount", PlayerRuntimeData.GetInstance().data.AttackData.Ammunition / _maxMunitions);
        _barMaterial.SetFloat("_GradientValue", 1 - PlayerRuntimeData.GetInstance().data.AttackData.Ammunition / _maxMunitions);
    }
}
