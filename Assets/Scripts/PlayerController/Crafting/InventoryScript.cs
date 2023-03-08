using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript _instance;
    public LeoScrolling _scroll;
    [SerializeField]
    PlayerAttack m_attack;
    public List<ProjectileData> projectilesData;
    public GameObject UI_Inventory;
    public GameObject SlotsContainer;
    public List<ProjectileData> recipe;

    [SerializeField]
    int numberOfIngredients;


    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        _instance = this;
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(numberOfIngredients);
            Craft();
        }
    }
    public void Craft()
    {

        if (recipe.Count < numberOfIngredients)
            return;
        /*
        //Fusionne les proprietes des differents ingredients
        for (int i = 0; i < numberOfIngredients; i++)
        {
            int rand = Random.Range(0, projectilesData.Count);
            recipe.Add(projectilesData[rand]);
            projectilesData.RemoveAt(rand);
        }
        */
        //Rafraichit l'affichage de l'inventaire
        RefreshInventoryUI();

        //Reinitialise les parametres de l'attaque precedente
        m_attack.ResetParameters();

        //Fusionne les effets et stats des differents ingredients
        foreach (ProjectileData ingredient in recipe)
        {

            m_attack._size += ingredient._size;
            m_attack._speed += ingredient._speed;
            m_attack._drag += ingredient._drag;
            m_attack._heavyAttackDelay += ingredient._heavyAttackDelay;
            m_attack._heavyDamage += ingredient._heavyDamage;
            m_attack._lightAttackDelay += ingredient._lightAttackDelay;
            m_attack._lightDamage += ingredient._lightDamage;

            //AJOUT DES EFFETS DANS LE SCRIPT D'ATTAQUE
            foreach(IIngredientEffects effect in ingredient._effects)
            {
                m_attack._effects.Add(effect);
            }
            

        }

        foreach (IIngredientEffects effect in m_attack._effects)
        {
            if (effect is MultipleShots)
            {
                MultipleShots TempEffect = (MultipleShots)effect;
                m_attack._ProjectileNbr = TempEffect._shotNbr;
                m_attack._TimeBtwShotsRafale = TempEffect._TimebtwShots;
            }
        }
        //Clear la Liste d'ingredients
        recipe.Clear();
       
    }
    public void AddIngredientToInventory(ProjectileData data)
    {
        projectilesData.Add(data);
        _scroll.ReloadUI();
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        //Rafraichit l'affichage de l'inventaire
    }
}