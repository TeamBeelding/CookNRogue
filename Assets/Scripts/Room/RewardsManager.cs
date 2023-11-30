using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardsManager : MonoBehaviour
{
    [Serializable]
    public class ItemDrop
    {
        public GameObject itemPrefab;
        public int minAmount = 1;
        public int maxAmount = 1;
    }
    [Serializable]
    public class ItemDropAmount
    {
        public int amount = 1;
    }



    [SerializeField]
    private Transform[] _dropPositions;

    [Space]

    [SerializeField]
    [Probability("_amountOfItemsDrop", "Beach")]
    private float[] _amountOfItemsDropProbas;

    [SerializeField]
    private ItemDropAmount[] _amountOfItemsDrop;

    [Space]

    [SerializeField]
    [Probability("_itemsToDrop", "Beach")]
    private float[] _itemsProbas;

    [SerializeField]
    private ItemDrop[] _itemsToDrop;

    private List<string> _pickedItems = new();


    [Space]


    [SerializeField]
    [Probability("_upgradesToDrop", "Beach")]
    private float[] _upgradesProbas;

    [SerializeField]
    private Item[] _upgradesToDrop;

    private void Awake()
    {
        if (_itemsToDrop.Length > 0)
        {
            EnemyManager.Instance.OnAllEnnemiesKilled += DropItems;
        }
        if (_upgradesToDrop.Length > 0)
        {
            EnemyManager.Instance.OnAllEnnemiesKilled += DropUpgrade;
        }
    }

    private void DropUpgrade()
    {
        SpawnRandomUpgrade();
    }


    private void DropItems()
    {
        _pickedItems.Clear();

        if (_amountOfItemsDrop.IsNullOrEmpty())
        {
            Debug.LogError("No amount of items to drop is defined in RewardsManager");
            return;
        }

        float random = Random.Range(0f, 1f);
        int amountOfItemsToDrop = _amountOfItemsDrop.Last().amount;
        for (int i = 0; i < _amountOfItemsDropProbas.Length; i++)
        {
            if (random < _amountOfItemsDropProbas[i])
            {
                amountOfItemsToDrop = _amountOfItemsDrop[i].amount;
                break;
            }
        }

        Debug.Log("Spawning " + amountOfItemsToDrop + " reward items");
        for (int i = 0; i < amountOfItemsToDrop; i++)
        {
            if (!SpawnRandomItem())
                break;
        }
    }

    /*
     * @returns Is spawning a success
     */
    private bool SpawnRandomItem()
    {
        Debug.Log("Spawning a reward item");
        if (_pickedItems.Count >= _itemsToDrop.Length)
        {
            Debug.LogError("No more reward left to pick from");
            return false;
        }

        float random = Random.Range(0f, 1f);
        ItemDrop randomItem = _itemsToDrop.Last();

        for (int i = 0; i < _itemsProbas.Length; i++)
        {
            if (random >= _itemsProbas[i])
                continue;

            var item = _itemsToDrop[i];
            if(_pickedItems.Contains(item.itemPrefab.name))
                continue;

            randomItem = item;
            break;
        }

        if (randomItem == null)
        {
            Debug.LogError("Couldn't pick any reward to spawn");
            return false;
        }

        if(_pickedItems.Contains(randomItem.itemPrefab.name))
            return false;

        _pickedItems.Add(randomItem.itemPrefab.name);

        var amountToSpawn = Random.Range(randomItem.minAmount, randomItem.maxAmount + 1);
        for (int i = 0; i < amountToSpawn; i++)
        {
            var inst = Instantiate(randomItem.itemPrefab, GetRandomSpawnPoint());
            inst.transform.SetParent(null);
        }

        Debug.Log("Spawned reward item " + randomItem.itemPrefab.name);

        return true;
    }

    private bool SpawnRandomUpgrade()
    {
        Debug.Log("Spawning a reward upgrade");

        float random = Random.Range(0f, 1f);
        Item randomUpgrade = _upgradesToDrop.Last();

        for (int i = 0; i < _upgradesProbas.Length; i++)
        {
            if (random >= _upgradesProbas[i])
                continue;

            var upgrade = _upgradesToDrop[i];
            
            if(upgrade.AlreadyHasUpgrade())
                continue;

            randomUpgrade = upgrade;
            break;
        }

        if (randomUpgrade == null)
        {
            Debug.LogError("Couldn't pick any reward to spawn");
            return false;
        }

        if(randomUpgrade.AlreadyHasUpgrade())
            return false;


        var inst = Instantiate(randomUpgrade.gameObject, GetRandomSpawnPoint());
        inst.transform.localPosition = Vector3.zero;
        inst.transform.SetParent(null);
        inst.transform.localRotation = Quaternion.identity;
        inst.transform.localScale = Vector3.one;

        Debug.Log("Spawned upgrade item " + randomUpgrade.gameObject.name);

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
