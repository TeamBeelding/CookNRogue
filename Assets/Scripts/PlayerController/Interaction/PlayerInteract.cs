using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    Item _interactable;
    private void OnTriggerEnter(Collider collision)
    {
        ISubItem Item = collision.GetComponent<ISubItem>();

        if (Item == null)
            return;

        Item.TriggerItem();

        /*
        if (Item == null || Item == _interactable)
            return;

        _interactable = collision.GetComponent<Item>();
        _interactable.ShowItemGFX();
        */
    }

    private void OnTriggerExit(Collider collision)
    {
        /*
        _interactable.HideItemGFX();
        _interactable = null;
        */
    }

    public void TryInteract(InputAction.CallbackContext context)
    {
        if(!context.performed) return;

        if (_interactable == null)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            //BUY ITEM
        }
            
    }
}
