using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

[System.Serializable]
public class PlayerCookingInventorySlot : MonoBehaviour
{
    [SerializeField, Required]
    ProjectileData m_data;

    [SerializeField, Required]
    Image m_ingredientImage;
 
    [SerializeField, Required]
    TextMeshProUGUI m_ingredientCounter;

    int _ingredientCount;
    RectTransform _transform;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_ingredientImage.sprite = m_data.inventorySprite;
    }

    public ProjectileData GetData()
    {
        return m_data;
    }

    public void Highlight(bool value)
    {
        if (value)
        {
            _transform.localScale = new Vector3(1.25f, 1.25f, 1);
        }
        else
        {
            _transform.localScale = new Vector3(1f, 1f, 1);
        }
    }

    public bool CanSelect()
    {
        return (_ingredientCount > 0);
    }

    #region Count management
    public void IncreaseCount()
    {
        _ingredientCount += 1;
        m_ingredientCounter.text = _ingredientCount.ToString();
    }

    public void DecreaseCount()
    {
        if(_ingredientCount <= 0)
        {
            return;
        }

        _ingredientCount += -1;
        m_ingredientCounter.text = _ingredientCount.ToString();
    }
    #endregion
}
