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

    [SerializeField, Required]
    Image m_backgroundImage;
    [SerializeField, Required]
    Image m_backgroundCounterImage;

    [SerializeField, Required]
    GameObject m_counterArrow;

    [Header("ImageBank")]
    [SerializeField, Required]
    Sprite m_backgroundHighlight;
    [SerializeField, Required]
    Sprite m_backgroundSelected;
    [SerializeField, Required]
    Sprite m_backgroundDefault;

    [SerializeField, Required]
    Sprite m_counterHighlight;
    [SerializeField, Required]
    Sprite m_counterSelected;
    [SerializeField, Required]
    Sprite m_counterDefault;

    [HideInInspector]
    public int _ingredientCount;
    RectTransform _transform;
    bool _isSelected;

    Color _greyedColor;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_ingredientImage.sprite = m_data.inventorySprite;
        _greyedColor = Color.gray;
        _greyedColor.a = 0.75f;
        m_ingredientImage.color = _greyedColor;
        m_counterArrow.SetActive(false);
    }

    public ProjectileData GetData()
    {
        return m_data;
    }

    public void Highlight(bool value)
    {
        if (value)
        {
            if (!_isSelected)
            {
                m_backgroundImage.sprite = m_backgroundHighlight;
                m_backgroundCounterImage.sprite = m_counterHighlight;
                _transform.localScale = new Vector3(1.25f, 1.25f, 1);
            }
            m_counterArrow.SetActive(true);
        }
        else
        {
            if (!_isSelected)
            {
                m_backgroundImage.sprite = m_backgroundDefault;
                m_backgroundCounterImage.sprite = m_counterDefault;
                _transform.localScale = new Vector3(1f, 1f, 1);
            }
            m_counterArrow.SetActive(false);
        }
    }

    public void Selected(bool value)
    {
        _isSelected = value;

        if (value)
        {
            m_backgroundImage.sprite = m_backgroundSelected;
            m_backgroundCounterImage.sprite = m_counterSelected;
            if (_ingredientCount <= 0)
            {
                m_ingredientImage.color = Color.white;
            }
        }
        else
        {
            m_backgroundImage.sprite = m_backgroundHighlight;
            m_backgroundCounterImage.sprite = m_counterHighlight;
            if (_ingredientCount <= 0)
            {
                m_ingredientImage.color = _greyedColor;
            }
        }
    }

    public bool CanSelect()
    {
        return (_ingredientCount > 0);
    }

    #region Count management
    public void IncreaseCount()
    {
        if (_ingredientCount <= 0 && !_isSelected)
        {
            m_ingredientImage.color = Color.white;
        }

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


        if (_ingredientCount <= 0 && !_isSelected)
        {
            m_ingredientImage.color = _greyedColor;
        }
    }

    public void ResetVisuals()
    {
        m_backgroundImage.sprite = m_backgroundDefault;
        m_backgroundCounterImage.sprite = m_counterDefault;
        _transform.localScale = new Vector3(1f, 1f, 1);
        m_counterArrow.SetActive(false);

        if (_ingredientCount <= 0)
        {
            m_ingredientImage.color = _greyedColor;
        }
        else
        {
            m_ingredientImage.color = Color.white;
        }
    }

    public void ResetCount()
    {
        m_ingredientImage.color = _greyedColor;

        _ingredientCount = 0;
        m_ingredientCounter.text = "0";
    }
    #endregion
}
