using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public IngredientData[] ingredients;
    public GameObject UI_Inventory;
    public GameObject SlotsContainer;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!UI_Inventory.activeInHierarchy)
            {
                UI_Inventory.SetActive(true);
            }
            else
            {
                UI_Inventory.SetActive(false);
            }
        }
    }
}
