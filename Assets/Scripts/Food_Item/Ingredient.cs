using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ingredient : MonoBehaviour , IInteractable
{
    public ProjectileData projectileData;

    public void Interactable(bool isInteractable)
    {
        //Nothing yet
    }
    public void Interact(string tag)
    {
        InventoryScript.instance.AddIngredientToList(projectileData);
        Destroy(gameObject);
    }

}
