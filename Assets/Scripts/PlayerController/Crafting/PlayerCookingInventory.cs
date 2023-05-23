using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCookingInventory : MonoBehaviour
{
    #region Variables
    [SerializeField]
    float m_showAnimDuration = 1f;

    [SerializeField]
    AnimationCurve m_showAnimPosCurve;

    [SerializeField] 
    List<PlayerCookingInventoryWheel> m_inventoryWheels;

    [SerializeField]
    List<Image> m_RecipeSlots;

    [SerializeField]
    RectTransform m_UIHolder;

    [SerializeField]
    PlayerAttack m_playerAttackScript;

    List<ProjectileData> _recipe;

    int _currentWheelIndex = 0;
    PlayerCookingInventorySlot _currentSlot;

    static PlayerCookingInventory _instance;

    bool _areControlsLocked;
    Vector3 _shownPosition;
    Vector3 _hiddenPosition;
    float _curAnimProgress;
    Coroutine _curShowRoutine;

    public static PlayerCookingInventory Instance
    {
        get => _instance;
    }
    #endregion

    private void Awake()
    {
        //Set instance
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
        _recipe = new List<ProjectileData>();

        _shownPosition = m_UIHolder.position;
        _hiddenPosition = _shownPosition;
        _hiddenPosition.y -= gameObject.GetComponent<RectTransform>().rect.height;
        m_UIHolder.position = _hiddenPosition;
    }

    #region Visuals
    public void Show(bool value)
    {
        if (value)
        {
            gameObject.SetActive(true);
            if(_curShowRoutine != null)
            {
                StopCoroutine(_curShowRoutine);
            }
            _curShowRoutine = StartCoroutine(ShowAnimation());
        }
        else
        {
            if (_curShowRoutine != null)
            {
                StopCoroutine(_curShowRoutine);
            }
            _curShowRoutine = StartCoroutine(HideAnimation());
        }
    }

    IEnumerator ShowAnimation()
    {
        _areControlsLocked = true;

        float time = m_showAnimDuration;
        Vector3 targetPos = _shownPosition;
        Vector3 initPos = m_UIHolder.position;


        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            m_UIHolder.position = Vector3.Lerp(initPos, targetPos, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Time.timeScale = Mathf.Lerp(1, 0, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            yield return null;
        }

        m_UIHolder.position = targetPos;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
    }

    IEnumerator HideAnimation()
    {
        _areControlsLocked = true;

        float time = m_showAnimDuration;
        Vector3 initPos = m_UIHolder.position;
        Vector3 targetPos = _hiddenPosition;

        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            m_UIHolder.position = Vector3.Lerp(targetPos, initPos, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Time.timeScale = Mathf.Lerp(1, 0, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            yield return null;
        }

        m_UIHolder.position = targetPos;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
        gameObject.SetActive(false);
    }

    public void SwitchWheel()
    {
        if (_areControlsLocked)
        {
            return;
        }

        //Multiple wheels check
        if (m_inventoryWheels == null || m_inventoryWheels.Count < 2)
        {
            Debug.Log("No inventory wheel to switch to");
            return;
        }

        //Deactivate current wheel
        m_inventoryWheels[_currentWheelIndex].SetActive(false);

        //Get next wheel
        if(_currentWheelIndex + 1 >= m_inventoryWheels.Count)
        {
            _currentWheelIndex = 0;
        }
        else
        {
            _currentWheelIndex += 1;
        }

        //Activate next wheel
        m_inventoryWheels[_currentWheelIndex].SetActive(true);
    }
    #endregion

    #region Inventory Management
    public void AddIngredientToInventory(ProjectileData data)
    {
        foreach(PlayerCookingInventoryWheel wheel in m_inventoryWheels)
        {
            for(int i = 0; i < 8; i++)
            {
                PlayerCookingInventorySlot slot = wheel.GetSlot(i);
                if(slot.GetData() == data)
                {
                    slot.IncreaseCount();
                    return;
                }
            }
        }
    }
    #endregion

    #region Bullet Crafting
    public void CancelCraft()
    {
        if(_recipe == null)
        {
            return;
        }

        //Reset recipe
        foreach (ProjectileData ingredient in _recipe)
        {
            AddIngredientToInventory(ingredient);
        }

        _recipe.Clear();

        //Reset UI
        foreach (Image image in m_RecipeSlots)
        {
            image.sprite = null;
        }
    }

    public void CraftBullet()
    {
        if (_areControlsLocked)
        {
            return;
        }

        if (_recipe.Count <= 0)
        {
            Debug.Log("Recipe is empty");
            return;
        }

        //Fuse ingredients's effects and stats
        foreach (ProjectileData ingredient in _recipe)
        {

            m_playerAttackScript._size += ingredient._size;
            m_playerAttackScript._speed += ingredient._speed;
            m_playerAttackScript._drag += ingredient._drag;
            m_playerAttackScript._shootCooldown += ingredient._attackDelay;
            m_playerAttackScript._damage += ingredient._damage;
            
            //Add effects
            foreach (IIngredientEffects effect in ingredient.Effects)
            {
                if(effect != null)
                {
                    Debug.Log("fffff");
                    m_playerAttackScript._effects.Add(effect);
                }
            }
        }

        //Average rate of fire
        m_playerAttackScript._shootCooldown /= _recipe.Count;

        foreach (IIngredientEffects effect in m_playerAttackScript._effects)
        {
            if (effect is MultipleShots)
            {
                MultipleShots TempEffect = (MultipleShots)effect;
                m_playerAttackScript._ProjectileNbr = TempEffect._shotNbr;
                m_playerAttackScript._TimeBtwShotsRafale = TempEffect._TimebtwShots;
            }
        }
        //Clear recipe
        _recipe.Clear();
    }
    #endregion

    #region Ingredient Selection
    public void OnSelectorInput(Vector2 input)
    {
        if (_areControlsLocked)
        {
            return;
        }

        //Get linear input from vectorial
        float correctedInput = OffsetInput(TranslateInput(input), 1 / (m_inventoryWheels[_currentWheelIndex].SlotsNumber() *2));
        //Get slot from linear input
        PlayerCookingInventorySlot slot = GetSlotFromInput(correctedInput);

        //Check for slot change
        if(slot != _currentSlot)
        {
            if(_currentSlot != null)
            {
                _currentSlot.Highlight(false);
            }

            slot.Highlight(true);

            _currentSlot = slot;
        }
    }

    public void OnSelectorInputStop()
    {
        if (_areControlsLocked)
        {
            return;
        }

        _currentSlot.Highlight(false);
        _currentSlot = null;
    }

    public void SelectIngredient()
    {
        if (_areControlsLocked)
        {
            return;
        }

        if (!_currentSlot)
        {
            return;
        }
        AddToRecipe(_currentSlot);
    }

    private float TranslateInput(Vector2 input)
    {
        if(input.x >= 0)
        {
            if(input.y >= 0)
            {
                return Mathf.Lerp(0f, 0.25f, input.x);
            }
            else
            {
                return Mathf.Lerp(0.25f, 0.5f, - input.y);
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
                return Mathf.Lerp(0.5f, 0.75f, - input.x);
            }
        }
    }

    private float OffsetInput(float input, float offset)
    {
        float newInput = input + offset;

        //Loop if negative
        if(newInput < 0)
        {
            newInput += 1;
        }
        //Loop if superior to max
        else if(newInput > 1)
        {
            newInput += -1;
        }

        return newInput;
    }

    private PlayerCookingInventorySlot GetSlotFromInput(float input)
    {
        float floatIndex = Mathf.Lerp(-1, 7, input);
        int index = Mathf.CeilToInt(floatIndex);

        PlayerCookingInventoryWheel curWheel = m_inventoryWheels[_currentWheelIndex];
        return curWheel.GetSlot(index);
    }

    private void AddToRecipe(PlayerCookingInventorySlot selectedSlot)
    {
        if (_recipe !=null && _recipe.Count >= m_RecipeSlots.Count || !selectedSlot.CanSelect())
        {
            Debug.Log("Can't select item");
            return;
        }

        selectedSlot.DecreaseCount();

        ProjectileData data = selectedSlot.GetData();
        _recipe.Add(data);
        m_RecipeSlots[_recipe.Count - 1].sprite = data.inventorySprite;
        
    }
    #endregion

    #region Utility
    public int RecipeSize()
    {
        return _recipe.Count;
    }

    public bool IsDisplayed()
    {
        return gameObject.activeSelf;
    }
    #endregion

    [System.Serializable]
    private class PlayerCookingInventoryWheel
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


