using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class TotemSelectorScript : MonoBehaviour
{
    #region Variables
    public static TotemSelectorScript instance;

    [SerializeField]
    List<PlayerInventoryWheel> m_inventoryWheels;

    [SerializeField]
    List<PlayerCookingRecipeSlot> m_recipeSlots;

    [SerializeField]
    GameObject m_navigateKey;
    [SerializeField]
    GameObject m_selectKey;
    [SerializeField]
    GameObject m_craftKey;

    int _currentWheelIndex = 0;
    PlayerCookingInventorySlot _currentSlot;


    Vector3 _shownPosition;
    Vector3 _hiddenPosition;
    float _curAnimProgress;
    Coroutine _curShowRoutine;



    public static TotemSelectorScript Instance
    {
        get => instance;
    }

    
    #endregion


    private void Awake()
    {
        //Set instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void Init()
    {
        ResetRecipeUI();

        for (int i = 0; i < m_inventoryWheels[0].SlotsNumber(); i++)
        {
            m_inventoryWheels[0].GetSlot(i)._ingredientCount = 0;

            for (int j = 0;j < PlayerCookingInventory.Instance.GetWheel().GetSlot(i)._ingredientCount; j++)
            {
                m_inventoryWheels[0].GetSlot(i).IncreaseCount();
            }
            
        }    
    }

    public void ApplyTotemHeal()
    {
        PlayerController.Instance.GetComponent<PlayerHealth>().Heal(PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count);
        PlayerRuntimeData.GetInstance().data.CookData.Recipe.Clear();
        ResetRecipeUI();
    }

    #region Ingredient Selection
    public void OnSelectorInput(Vector2 input)
    {


        if (m_navigateKey.activeSelf)
        {
            m_navigateKey.SetActive(false);
            m_selectKey.SetActive(true);
        }

        //Get linear input from vectorial
        float correctedInput = OffsetInput(TranslateInput(input), 1 / (m_inventoryWheels[_currentWheelIndex].SlotsNumber() * 2));
        //Get slot from linear input
        PlayerCookingInventorySlot slot = GetSlotFromInput(correctedInput);

        //Check for slot change
        if (slot != _currentSlot)
        {
            if (_currentSlot != null)
            {
                _currentSlot.Highlight(false);
            }

            //Item Description
            if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count < m_recipeSlots.Count)
            {
                ProjectileData slotData = slot.GetData();
                m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Sprite = slotData.inventorySprite;
                m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Color = Color.white;
                m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Description = slotData.description;
            }

            slot.Highlight(true);
            _currentSlot = slot;
        }
    }

    public void OnSelectorInputStop()
    {


        _currentSlot.Highlight(false);
        _currentSlot = null;

        m_navigateKey.SetActive(true);
        m_selectKey.SetActive(false);
    }

    public void SelectIngredient()
    {

        if (!_currentSlot)
        {
            return;
        }

        if (!m_craftKey.activeSelf)
        {
            m_craftKey.SetActive(true);
        }

        AddToRecipe(_currentSlot);
    }
    public void ResetRecipeUI()
    {
        foreach (PlayerCookingRecipeSlot slot in m_recipeSlots)
        {
            slot.Sprite = null;
            slot.Color = new(1, 1, 1, 0);
            slot.Description = null;
        }

        m_navigateKey.SetActive(true);
        m_selectKey.SetActive(false);
        m_craftKey.SetActive(false);
    }
    private float TranslateInput(Vector2 input)
    {
        if (input.x >= 0)
        {
            if (input.y >= 0)
            {
                return Mathf.Lerp(0f, 0.25f, input.x);
            }
            else
            {
                return Mathf.Lerp(0.25f, 0.5f, -input.y);
            }
        }
        else
        {
            if (input.y >= 0)
            {
                return Mathf.Lerp(0.75f, 1f, input.y);
            }
            else
            {
                return Mathf.Lerp(0.5f, 0.75f, -input.x);
            }
        }
    }

    private float OffsetInput(float input, float offset)
    {
        float newInput = input + offset;

        //Loop if negative
        if (newInput < 0)
        {
            newInput += 1;
        }
        //Loop if superior to max
        else if (newInput > 1)
        {
            newInput += -1;
        }

        return newInput;
    }

    private PlayerCookingInventorySlot GetSlotFromInput(float input)
    {
        float floatIndex = Mathf.Lerp(-1, 7, input);
        int index = Mathf.CeilToInt(floatIndex);

        PlayerInventoryWheel curWheel = m_inventoryWheels[_currentWheelIndex];
        return curWheel.GetSlot(index);
    }

    private void AddToRecipe(PlayerCookingInventorySlot selectedSlot)
    {
        if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count >= m_recipeSlots.Count || !selectedSlot.CanSelect())
        {
            Debug.Log("Can't select item");
            return;
        }

        ProjectileData data = selectedSlot.GetData();
        selectedSlot.DecreaseCount();

        for (int i = 0; i < PlayerCookingInventory.Instance.GetWheel().SlotsNumber(); i++)
        {
            if(PlayerCookingInventory.Instance.GetWheel().GetSlot(i).GetData() == data)
            {
                Debug.Log("DECREASE INVENTORY");
                PlayerCookingInventory.Instance.GetWheel().GetSlot(i).DecreaseCount();
            }
        }

        

        PlayerRuntimeData.GetInstance().data.CookData.Recipe.Add(data);
        m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].Sprite = data.inventorySprite;
        m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].Description = data.description;
    }

    #endregion

    [System.Serializable]
    private class PlayerInventoryWheel
    {
        [SerializeField]
        GameObject m_wheelObject;

        [SerializeField]
        List<PlayerCookingInventorySlot> m_slots;

        public void SetActive(bool value)
        {
            m_wheelObject.SetActive(value);
        }

        public int SlotsNumber()
        {
            return m_slots.Count;
        }

        public PlayerCookingInventorySlot GetSlot(int index)
        {
            if (index < m_slots.Count && index >= 0)
            {
                return m_slots[index];
            }
            else
            {
                Debug.Log("Inventory slot index outside the array");
                return null;
            }
        }
    }
}
