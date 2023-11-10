using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeReference] ItemData data;
    private int cost;
    private string name;
    private string description;
    [SerializeReference] GameObject GFX;
    protected bool _hasTriggered = false;

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
    protected bool CanTrigger()
    {
        if (!_hasTriggered)
        {
            _hasTriggered = true;
            return true;
        }

        return false;
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

    protected void ApplyItemRoutine(){ StartCoroutine(ToPlayer()); }

    public IEnumerator ToPlayer()
    {
        Debug.Log("ToPlayer");
        Transform playerTransform = PlayerHealth.instance.transform;
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        while (distanceToPlayer > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, 0.05f);
            distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            yield return new WaitForEndOfFrame();
        }
        //DESTROY
    }
}
