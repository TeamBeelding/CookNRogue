using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNRD;


public class Ingredient : MonoBehaviour, IInteractable
{
    public ProjectileData projectileData;
    [SerializeField] private List<SerializableInterface<IIngredientEffects>> effects;

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
        if(other.tag == "Player")
        {
            //Add effect to projectile data
            var clone = Instantiate(projectileData);
            clone.effects = new List<IIngredientEffects>();
            foreach (SerializableInterface<IIngredientEffects> effect in effects)
            {
                clone.effects.Add(effect.Value);
            }

            InventoryScript.instance.AddIngredientToList(clone);
            Destroy(gameObject);
        }

    }

}
