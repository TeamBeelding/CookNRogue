using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;
    [SerializeField]
    PlayerAttack attack;
    public List<ProjectileData> projectilesData;
    public GameObject UI_Inventory;
    public GameObject SlotsContainer;
    List<ProjectileData> recipe;

    [SerializeField]
    int numberOfIngredients;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        instance = this;
    }
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
        //Clear la Liste d'ingredients precedente
        recipe.Clear();

        //Fusionne les proprietes des differents ingredients
        for (int i = 0;i < numberOfIngredients; i++)
        {
            int rand = Random.Range(0, projectilesData.Count);
            recipe.Add(projectilesData[rand]);
            projectilesData.RemoveAt(rand);
        }

        //Rafraichit l'affichage de l'inventaire
        RefreshInventoryUI();

        //Reinitialise les parametres de l'attaque precedente
        attack.ResetParameters();

        //Fusionne les effets et stats des differents ingredients
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
    public void AddIngredientToList(ProjectileData data)
    {
        projectilesData.Add(data);
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        //Rafraichit l'affichage de l'inventaire
    }
}