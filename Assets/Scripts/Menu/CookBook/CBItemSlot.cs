#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CBItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image m_itemImage;

    [SerializeField]
    private TextMeshProUGUI m_itemName;

    [SerializeField]
    private TextMeshProUGUI m_itemDescription;

    [SerializeField]
    private ItemData m_itemData;

    private ItemData _curData;
    Sprite Sprite
    {
        set
        {
            m_itemImage.sprite = value;
        }
    }

    string Name
    {
        set
        {
            m_itemName.text = value;
        }
    }

    string Description
    {
        set
        {
            m_itemDescription.text = value;
        }
    }

    private void OnValidate()
    {
        if (m_itemData != null)
        {
            if (_curData != null && _curData == m_itemData)
            {
                return;
            }

            _curData = m_itemData;
            Sprite = m_itemData.inventorySprite;
            Name = m_itemData.name;
            Description = m_itemData.description;
        }
    }
}
#endif
