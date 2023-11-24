using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    private static PlayerInventoryScript _instance;
    public PlayerInventoryScrolling _scroll;
    public List<ProjectileData> projectilesData;
    public GameObject UI_Inventory;
    public List<ProjectileData> recipe;

    [SerializeField]
    int numberOfIngredients;

    public bool IsDisplayed
    {
        get
        {
            return UI_Inventory.activeInHierarchy;
        }
    }

    public static PlayerInventoryScript Instance 
    {
        get { return _instance; }
    }


    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        _instance = this;
    }
    private void Start()
    {
        recipe = new List<ProjectileData>();
        Show(false);
    }

    public void Show(bool value)
    {
        UI_Inventory.SetActive(value);

        //Rafraichit l'affichage de l'inventaire
        RefreshInventoryUI();
    }

    public void SelectIngredient()
    {
        _scroll.SelectIngredient();
    }

    public void CancelCraft()
    {
        foreach (ProjectileData ingredient in recipe)
        {
            AddIngredientToInventory(ingredient);
        }

        recipe.Clear();
    }

    public void CraftBullet()
    {
        if(recipe.Count <= 0) 
        {
            Debug.Log("Recipe is empty");
            return;
        }

        //PREPARE LE DELAI DE TIR EN LE METTANT A ZERO POUR IGNORER LA VALEUR DE BASE
        PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown = 0;
        //Fusionne les effets et stats des differents ingredients
        foreach (ProjectileData ingredient in recipe)
        {

            PlayerRuntimeData.GetInstance().data.AttackData.AttackSize += ingredient._size;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed += ingredient._speed;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag += ingredient._drag;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage += ingredient._damage;
            PlayerRuntimeData.GetInstance().data.AttackData.Ammunition += ingredient._ammunition;
            //m_attack._shootCooldown += ingredient._attackDelay;

            if (AmmunitionBar.instance!= null)
                AmmunitionBar.instance.AddIngredientAmmo(ingredient._ammunition);

            //AJOUT DES EFFETS DANS LE SCRIPT D'ATTAQUE
            foreach (IIngredientEffects effect in ingredient.Effects)
            {
                PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects.Add(effect);
            }
        }
        //CALCULE LE DELAIS DE TIR ENTRE CHAQUE BALLE EN FAISANT LA MOYENNE
        //m_attack.CalculateShootDelay();

        foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
        {
            if (effect is MultipleShots)
            {
                MultipleShots TempEffect = (MultipleShots)effect;
                PlayerRuntimeData.GetInstance().data.AttackData.ProjectileNumber = TempEffect._shotNbr;
                PlayerRuntimeData.GetInstance().data.AttackData.TimeBtwShotRafale = TempEffect._TimebtwShots;
            }
        }
        //Clear la Liste d'ingredients
        recipe.Clear();
    }

    public void AddIngredientToInventory(ProjectileData data)
    {
        projectilesData.Add(data);
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        _scroll.ReloadUI();
    }
}