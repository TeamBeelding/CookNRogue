using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNRD;


public class Ingredient : MonoBehaviour, IInteractable
{
    public ProjectileData _projectileData;
    [SerializeField]
    private List<SerializableInterface<IIngredientEffects>> m_effects;

    public void Interactable(bool isInteractable)
    {
        //Nothing yet
    }
    public void Interact(string tag)
    {
        PlayerInventoryScript.Instance.AddIngredientToInventory(_projectileData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Add effect to projectile data
            var clone = Instantiate(_projectileData);
            clone._effects = new List<IIngredientEffects>();
            foreach (SerializableInterface<IIngredientEffects> effect in m_effects)
            {
                clone._effects.Add(effect.Value);
            }

            PlayerInventoryScript.Instance.AddIngredientToInventory(clone);
            Destroy(gameObject);
        }

    }

}
