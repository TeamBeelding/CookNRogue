using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField]
    AttackTest attack;
    public List<ProjectileData> projectilesData;
    public GameObject UI_Inventory;
    public GameObject SlotsContainer;
    List<ProjectileData> recipe;

    [SerializeField]
    int numberOfIngredients;

    private void Start()
    {
        recipe = new List<ProjectileData>();
    }
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

        if (Input.GetKeyDown(KeyCode.C) && projectilesData.Count >= numberOfIngredients)
        {
            Craft();
        }
    }
    void Craft()
    {
        recipe.Clear();
        for (int i = 0;i < numberOfIngredients; i++)
        {
            int rand = Random.Range(0, projectilesData.Count);
            recipe.Add(projectilesData[rand]);
            projectilesData.RemoveAt(rand);
        }
        attack.ResetParameters();
        foreach (ProjectileData ingredient in recipe)
        {
            attack.size += ingredient.size;
            attack.speed += ingredient.speed;
            attack.drag += ingredient.drag;
            attack.heavyAttackDelay += ingredient.heavyAttackDelay;
            attack.heavyDamage += ingredient.heavyDamage;
            attack.lightAttackDelay += ingredient.lightAttackDelay;
            attack.lightDamage += ingredient.lightDamage;
        }
    }
}
