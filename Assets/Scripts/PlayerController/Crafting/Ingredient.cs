using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNRD;


public class Ingredient : MonoBehaviour, IInteractable
{
    public ProjectileData projectileData;
    [SerializeField] private SerializableInterface<IEffects> effect;

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
        projectileData.effect = effect.Value;
        InventoryScript.instance.AddIngredientToList(projectileData);

        Destroy(gameObject);
    }

}
