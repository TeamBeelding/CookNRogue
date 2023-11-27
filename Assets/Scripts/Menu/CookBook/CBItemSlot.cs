#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CBItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image m_ingredientImage;

    [SerializeField]
    private TextMeshProUGUI m_ingredientName;

    [SerializeField]
    private TextMeshProUGUI m_ingredientDescription;

    [SerializeField]
    private ProjectileData m_ingredientData;

    private ProjectileData _curData;
    Sprite Sprite
    {
        set
        {
            m_ingredientImage.sprite = value;
        }
    }

    string Name
    {
        set
        {
            m_ingredientName.text = value;
        }
    }

    string Description
    {
        set
        {
            m_ingredientDescription.text = value;
        }
    }

    private void OnValidate()
    {
        if (m_ingredientData != null)
        {
            if (_curData != null && _curData == m_ingredientData)
            {
                return;
            }

            _curData = m_ingredientData;
            Sprite = m_ingredientData.inventorySprite;
            Name = m_ingredientData.ingredientName;
            Description = m_ingredientData.description;
        }
    }
}
#endif
