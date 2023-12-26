using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RewardsManager : MonoBehaviour
{
    //[Header("Sound")]
    //[SerializeField]
    //private AK.Wwise.Event _Play_SFX_Object_Appear;

    [Serializable]
    public class IngredientDrop
    {
        public GameObject ingredientPrefab;
        public int minAmount = 1;
        public int maxAmount = 1;
    }
    [Serializable]
    public class IngredientDropAmount
    {
        public int amount = 1;
    }


    [SerializeField]
    private Transform[] _dropPositions;

    [FormerlySerializedAs("_amountOfIngredientsDropProbas")]
    [Space]

    [SerializeField]
    [Probability("_amountOfIngredientsDrop", "Beach")]
    private float[] _amountOfIngredientsDropProbas;

    [FormerlySerializedAs("_amountOfIngredientsDrop")] [SerializeField]
    private IngredientDropAmount[] _amountOfIngredientsDrop;

    [FormerlySerializedAs("_ingredientsProbas")]
    [Space]

    [SerializeField]
    [Probability("_ingredientsToDrop", "Beach")]
    private float[] _ingredientsProbas;

    [FormerlySerializedAs("_ingredientsToDrop")] [SerializeField]
    private IngredientDrop[] _ingredientsToDrop;

    private List<string> _pickedIngredients = new();


    [FormerlySerializedAs("_itemsProbas")]
    [Space]


    [SerializeField]
    [Probability("_itemsToDrop", "Beach")]
    private float[] _itemsProbas;

    [FormerlySerializedAs("_itemsToDrop")] [SerializeField]
    private Item[] _itemsToDrop;

    private void Awake()
    {
        if (_ingredientsToDrop.Length > 0)
        {
            EnemyManager.Instance.OnAllEnnemiesKilled += DropIngredients;
        }
        if (_itemsToDrop.Length > 0)
        {
            EnemyManager.Instance.OnAllEnnemiesKilled += DropItem;
        }
    }

    private void DropItem()
    {
        SpawnRandomItem();
    }


    private void DropIngredients()
    {
        _pickedIngredients.Clear();

        if (_amountOfIngredientsDrop.IsNullOrEmpty())
        {
            Debug.LogError("No amount of ingredient to drop is defined in RewardsManager");
            return;
        }

        float random = Random.Range(0f, 1f);
        int amountOfIngredientsToDrop = _amountOfIngredientsDrop.Last().amount;
        for (int i = 0; i < _amountOfIngredientsDropProbas.Length; i++)
        {
            if (random < _amountOfIngredientsDropProbas[i])
            {
                amountOfIngredientsToDrop = _amountOfIngredientsDrop[i].amount;
                break;
            }
        }

        Debug.Log("Spawning " + amountOfIngredientsToDrop + " reward items");
        for (int i = 0; i < amountOfIngredientsToDrop; i++)
        {
            if (!SpawnRandomIngredient())
                break;
        }
    }

    /*
     * @returns Is spawning a success
     */
    private bool SpawnRandomIngredient()
    {
        Debug.Log("Spawning a reward item");
        if (_pickedIngredients.Count >= _ingredientsToDrop.Length)
        {
            Debug.LogError("No more reward left to pick from");
            return false;
        }

        float random = Random.Range(0f, 1f);
        IngredientDrop randomIngredient = _ingredientsToDrop.Last();

        for (int i = 0; i < _ingredientsProbas.Length; i++)
        {
            if (random >= _ingredientsProbas[i])
                continue;

            var item = _ingredientsToDrop[i];
            if(_pickedIngredients.Contains(item.ingredientPrefab.name))
                continue;

            randomIngredient = item;
            break;
        }

        if (randomIngredient == null)
        {
            Debug.LogError("Couldn't pick any reward to spawn");
            return false;
        }

        if(_pickedIngredients.Contains(randomIngredient.ingredientPrefab.name))
            return false;

        _pickedIngredients.Add(randomIngredient.ingredientPrefab.name);

        var amountToSpawn = Random.Range(randomIngredient.minAmount, randomIngredient.maxAmount + 1);
        for (int i = 0; i < amountToSpawn; i++)
        {
            var inst = Instantiate(randomIngredient.ingredientPrefab, GetRandomSpawnPoint());
            inst.transform.localPosition = Vector3.zero;
            inst.transform.SetParent(null);
            inst.transform.localRotation = Quaternion.identity;
            inst.transform.localScale = Vector3.one;
        }

        Debug.Log("Spawned reward ingredient " + randomIngredient.ingredientPrefab.name);

        return true;
    }

    private bool SpawnRandomItem()
    {
        if (_itemsToDrop.IsNullOrEmpty())
        {
            Debug.Log("Tried to spawn an item but is empty");
            return false;
        }
        Debug.Log("Spawning a reward item");

        float random = Random.Range(0f, 1f);
        Item randomItem = _itemsToDrop.Last();

        for (int i = 0; i < _itemsProbas.Length; i++)
        {
            if (random >= _itemsProbas[i])
                continue;

            var item = _itemsToDrop[i];

            if(item.AlreadyHasUpgrade())
                continue;

            randomItem = item;
            break;
        }

        if (randomItem == null)
        {
            Debug.LogError("Couldn't pick any reward to spawn");
            return false;
        }

        if(randomItem.AlreadyHasUpgrade())
            return false;


        var inst = Instantiate(randomItem.gameObject, GetRandomSpawnPoint());
        inst.transform.localPosition = Vector3.zero;
        inst.transform.SetParent(null);
        inst.transform.localRotation = Quaternion.identity;
        inst.transform.localScale = Vector3.one;

        //_Play_SFX_Object_Appear.Post(gameObject);
        Debug.Log("Spawned reward item " + randomItem.gameObject.name);

        return true;
    }

    private Transform GetRandomSpawnPoint()
    {
        if (_dropPositions.Length == 0)
        {
            Debug.LogError("No reward drop position set");
            return null;
        }
        int random = Random.Range(0, _dropPositions.Length);
        return _dropPositions[random];
    }
}
