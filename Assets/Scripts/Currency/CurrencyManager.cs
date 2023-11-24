using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    [SerializeField] TextMeshProUGUI _currencyText;
    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        UpdateCurrencyText();
    }
    public void AddCurrency(int amount)
    {
        PlayerRuntimeData.GetInstance().data.InventoryData.Currency += amount;
        UpdateCurrencyText();
    }
    public bool CheckCurrency(int amount)
    {
        return (amount <= PlayerRuntimeData.GetInstance().data.InventoryData.Currency);
    }
    public void RemoveCurrency(int amount)
    {
        PlayerRuntimeData.GetInstance().data.InventoryData.Currency -= amount;
        UpdateCurrencyText();
    }
    private void UpdateCurrencyText()
    {
        if (_currencyText == null)
            return;

        _currencyText.text = PlayerRuntimeData.GetInstance().data.InventoryData.Currency.ToString();
    }
}
