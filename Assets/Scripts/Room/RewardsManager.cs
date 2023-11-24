using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
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
    private Transform _dropPosition;

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

    private void Awake()
    {
        EnemyManager.Instance.OnAllEnnemiesKilled += DropItems;
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

        var inst = Instantiate(randomItem.itemPrefab, _dropPosition);
        // inst.transform.SetParent(null);

        Debug.Log("Spawned reward item " + randomItem.itemPrefab.name);

        return true;
    }
}
