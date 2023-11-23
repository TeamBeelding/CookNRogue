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
    private TextMeshProUGUI m_ingredientDescription;

    public Sprite Sprite
    {
        set
        {
            m_ingredientImage.sprite = value;
        }
    }

    public Color Color
    {
        set
        {
            m_ingredientImage.color = value;
        }
    }

    public string Description
    {
        set
        {
            m_ingredientDescription.text = value;
        }
    }
}
