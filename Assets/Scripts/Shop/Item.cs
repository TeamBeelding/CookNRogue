using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,IInteractable
{
    [SerializeReference] ItemData data;
    private int cost;
    private string name;
    private string description;
    [SerializeReference] GameObject GFX;

    void Awake()
    {
        cost = data.cost;
        name = data.name;
        description = data.description;
    }

    public void ShowItemGFX()
    {
        GFX.SetActive(true);
    }

    public void HideItemGFX()
    {
        GFX.SetActive(false);
    }

    public void Interactable(bool isInteractable) { }

    public virtual void Interact(string tag)
    {
        bool hasEnoughCurrency = CurrencyManager.instance.CheckCurrency(cost);

        if (!hasEnoughCurrency)
            return;

        CurrencyManager.instance.RemoveCurrency(cost);
        Debug.Log("bought "+ name +" for "+  cost + " currency!");
    }

    public virtual void ApplyItem(){ }
}
