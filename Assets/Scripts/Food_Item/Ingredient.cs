using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ingredient : MonoBehaviour , IInteractable
{
    public ProjectileData projectileDataRef;

    [HideInInspector]
    public ProjectileData projectileData;
    void Start()
    {
        var clone = Instantiate(projectileData);
        projectileData = projectileDataRef;
        IEffects[] effects = GetComponents<IEffects>();
        foreach (IEffects effect in effects)
        {
            projectileData.effects.Add(effect);
        }
    }
    public void Interactable(bool isInteractable)
    {
        //Nothing yet
    }
    public void Interact(string tag)
    {
        InventoryScript.instance.AddIngredientToList(projectileData);
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        InventoryScript.instance.AddIngredientToList(projectileData);

        Destroy(gameObject);
    }

}
