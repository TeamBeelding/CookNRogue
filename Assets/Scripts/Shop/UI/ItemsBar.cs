using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemsBar : MonoBehaviour
{
    [SerializeField]
    private List<ItemsBarSlot> m_itemsSlots;

    private int index = 0;

    static ItemsBar _instance;

    public static ItemsBar Instance
    {
        get => _instance;
    }

    private void Awake()
    {
        //Set instance
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        foreach (ItemsBarSlot item in m_itemsSlots)
        {
            item.Set();
        }
    }

    public void AddItem(ItemData item)
    {
        foreach (ItemsBarSlot slot in m_itemsSlots)
        {
            if(slot.IsUsed && slot.data == item)
            {
                slot.IncreaseCount();
                return;
            }
        }

        if (index >= m_itemsSlots.Count)
            return;

        m_itemsSlots[index].AddItem(item);
        index += 1;
    }

    [System.Serializable]
    private class ItemsBarSlot
    {
        [SerializeField]
        GameObject m_slot;

        [SerializeField]
        Image m_sprite;

        [SerializeField]
        TextMeshProUGUI m_counter;

        [HideInInspector]
        public ItemData data;

        private int count = 0;

        public bool IsUsed
        {
            get
            {
                return data != null;
            }
        }

        public void Set()
        {
            m_counter.transform.parent.gameObject.SetActive(false);
            m_slot.SetActive(false);
        }

        public void AddItem(ItemData item)
        {
            data = item;
            m_sprite.sprite = data.inventorySprite;
            count = 1;
            m_slot.SetActive(true);
        }

        public void IncreaseCount()
        {
            if(count <= 1)
            {
                m_counter.transform.parent.gameObject.SetActive(true);
            }

            count += 1;
            m_counter.text = count.ToString();
        }
    }
}
