using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TotemInteraction : MonoBehaviour,IInteractable
{

    public void Interact(string tag)
    {
        if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count > 0)
        {
            PlayerCookingInventory.Instance.ResetRecipeUI();
            PlayerCookingInventory.Instance.CancelCraft();
        }

        TotemMenuManager._instance.SwitchUI(); 
    }

    public void Interactable(bool isInteractable)
    {

    }

}
