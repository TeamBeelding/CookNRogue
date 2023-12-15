using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TotemInteraction : MonoBehaviour,IInteractable
{
    [SerializeField] int _healPerIngredient = 1;
    public void Interact(string tag)
    {
        Regen();
    }

    public void Interactable(bool isInteractable)
    {

    }

    public void Regen()
    {
        if (PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth >= PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth)
            return;

        if (PlayerCookingInventory.Instance.RemoveRandomIngredient())
            PlayerHealth.instance.Heal(_healPerIngredient);
        else
            return;
    }
}
