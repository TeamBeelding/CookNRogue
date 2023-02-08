using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ingredient : MonoBehaviour , IInteractable
{
    public ProjectileData projectileData;
    public IEffects effect = new Knockback();
    
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
        //Add effect to projectile data
        projectileData.effect = effect;
        InventoryScript.instance.AddIngredientToList(projectileData);
        
        Destroy(gameObject);
    }

}
