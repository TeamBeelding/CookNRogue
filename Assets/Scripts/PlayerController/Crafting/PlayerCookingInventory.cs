using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AK.Wwise;

public class PlayerCookingInventory : MonoBehaviour
{
    #region Variables

    [SerializeField]
    AnimationCurve m_showAnimPosCurve;

    [SerializeField]
    Image m_transition;

    [SerializeField] 
    List<PlayerCookingInventoryWheel> m_inventoryWheels;

    [SerializeField]
    List<PlayerCookingRecipeSlot> m_recipeSlots;

    [SerializeField]
    RectTransform m_UIHolder;

    [SerializeField]
    GameObject m_navigateKey;
    [SerializeField]
    GameObject m_selectKey;
    [SerializeField]
    GameObject m_craftKey;

    [Header("Defautl UI")]
    [SerializeField]
    GameObject m_defaultUI;
    [SerializeField]
    List<PlayerCookingRecipeSlot> m_defaultRecipeSlots;

    [SerializeField] 
    float[] _damageFactor;

    int _currentWheelIndex = 0;
    PlayerCookingInventorySlot _currentSlot;

    static PlayerCookingInventory _instance;

    bool _areControlsLocked;
    Vector3 _shownPosition;
    Vector3 _hiddenPosition;
    float _curAnimProgress;
    Coroutine _curShowRoutine;

    List<ProjectileData> _equippedRecipe = new();

    [SerializeField] private AK.Wwise.Event _Play_SFX_Ingredient_Collect;
    [SerializeField] private GameObject Cooking_Particles;
    public List<ProjectileData> EquippedRecipe
    {
        get => _equippedRecipe;
    }

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

        _shownPosition = m_UIHolder.localPosition;
        _hiddenPosition = _shownPosition;
        _hiddenPosition.y -= gameObject.GetComponent<RectTransform>().rect.height;
        m_UIHolder.localPosition = _hiddenPosition;

        ResetRecipeUI();
    }

    #region Visuals
    public void Show(bool value)
    {
        if (value)
        {
            gameObject.SetActive(true);
            m_defaultUI.SetActive(false);

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

        float time = PlayerRuntimeData.GetInstance().data.CookData.ShowAnimDuration;
        Vector3 targetPos = _shownPosition;
        Vector3 initPos = m_UIHolder.localPosition;


        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            m_UIHolder.localPosition = Vector3.Lerp(initPos, targetPos, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Time.timeScale = Mathf.Lerp(1, 0, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Color tempc = m_transition.color;
            tempc.a = Mathf.Lerp(0, 0.9f, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_transition.color = tempc;
            yield return null;
        }

        m_UIHolder.localPosition = targetPos;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
    }

    IEnumerator HideAnimation()
    {
        _areControlsLocked = true;

        float time = PlayerRuntimeData.GetInstance().data.CookData.ShowAnimDuration;
        Vector3 initPos = m_UIHolder.localPosition;
        Vector3 targetPos = _hiddenPosition;

        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            m_UIHolder.localPosition = Vector3.Lerp(targetPos, initPos, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Time.timeScale = Mathf.Lerp(1, 0, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Color tempc = m_transition.color;
            tempc.a = Mathf.Lerp(0, 0.9f, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_transition.color = tempc;
            yield return null;
        }

        m_UIHolder.localPosition = targetPos;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
        gameObject.SetActive(false);
        m_defaultUI.SetActive(true);
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

    void PlayParticlesCooking()
    {

        if (Cooking_Particles == null)
            return;

        ParticleSystem[] particles = Cooking_Particles.transform.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem part in particles)
        {
            part.Play();
        }

    }
    #endregion

    #region Inventory Management
    public void AddIngredientToInventory(ProjectileData data)
    {
        _Play_SFX_Ingredient_Collect.Post(gameObject);

        foreach (PlayerCookingInventoryWheel wheel in m_inventoryWheels)
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

    public void Clear()
    {
        foreach (PlayerCookingInventoryWheel wheel in m_inventoryWheels)
        {
            for (int i = 0; i < 8; i++)
            {
                PlayerCookingInventorySlot slot = wheel.GetSlot(i);
                slot.ResetCount();
            }
        }
    }
    #endregion

    #region Bullet Crafting
    public void CancelCraft()
    {
        if(PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count == 0)
        {
            return;
        }

        //Reset recipe
        foreach (ProjectileData ingredient in PlayerRuntimeData.GetInstance().data.CookData.Recipe)
        {
            AddIngredientToInventory(ingredient);
        }

        PlayerRuntimeData.GetInstance().data.CookData.Recipe.Clear();

        ResetRecipeUI();
    }

    public void CraftBullet()
    {
        if (_areControlsLocked)
        {
            return;
        }

        PlayParticlesCooking();

        if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count <= 0)
        {
            Debug.Log("Recipe is empty");
            return;
        }


        PlayerRuntimeData.GetInstance().data.AttackData.AttackColor = PlayerRuntimeData.GetInstance().data.CookData.Recipe[0].color;
        AmmunitionBar.instance.ResetAmmoBar();
        _equippedRecipe.Clear();
        //Fuse ingredients's effects and stats
        float averageDmg = 0;
        foreach (ProjectileData ingredient in PlayerRuntimeData.GetInstance().data.CookData.Recipe)
        {
            PlayerRuntimeData.GetInstance().data.AttackData.AttackSize += ingredient._size;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed += ingredient._speed;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag += ingredient._drag;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown += ingredient._attackDelay;
            averageDmg += ingredient._damage;
            PlayerRuntimeData.GetInstance().data.AttackData.Ammunition += ingredient._ammunition;
            AmmunitionBar.instance.AddIngredientAmmo(ingredient._ammunition);

            //Audio
            ingredient.audioState.SetValue();
            EquippedRecipe.Add(ingredient);

            //Add effects
            foreach (IIngredientEffects effect in ingredient.Effects)
            {
                if(effect != null)
                {                   
                    PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects.Add(effect);
                }
            }
        }
        averageDmg /= PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count;

        switch (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count)
        {
            case 1:
                averageDmg *= _damageFactor[0];
                break;
            case 2:
                averageDmg *= _damageFactor[1];
                break;
            case 3:
                averageDmg *= _damageFactor[2];
                break;

        }
        PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage = averageDmg;

        //Average rate of fire
        PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown /= PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count;

        foreach (IIngredientEffects effect in PlayerRuntimeData.GetInstance().data.AttackData.AttackEffects)
        {
            if (effect is MultipleShots TempEffect)
            {
                PlayerRuntimeData.GetInstance().data.AttackData.ProjectileNumber = TempEffect._shotNbr;
                PlayerRuntimeData.GetInstance().data.AttackData.TimeBtwShotRafale = TempEffect._TimebtwShots;
            }
        }

        //Update default UI
        UpdateEquipedRecipeUI();

        //Clear recipe
        PlayerRuntimeData.GetInstance().data.CookData.Recipe.Clear();

        ResetRecipeUI();
    }
    #endregion

    #region Ingredient Selection
    public void OnSelectorInput(Vector2 input)
    {
        if (_areControlsLocked)
        {
            return;
        }

        if (m_navigateKey.activeSelf)
        {
            m_navigateKey.SetActive(false);
            m_selectKey.SetActive(true);
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
        if (_areControlsLocked)
        {
            return;
        }

        //Item Description
        if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count < m_recipeSlots.Count)
        {
            m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Sprite = null;
            m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Color = Color.clear;
            m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Description = null;
        }

        _currentSlot.Highlight(false);
        _currentSlot = null;

        m_navigateKey.SetActive(true);
        m_selectKey.SetActive(false);
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

        if (!m_craftKey.activeSelf)
        {
            m_craftKey.SetActive(true);
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
        if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count >= m_recipeSlots.Count || !selectedSlot.CanSelect())
        {
            Debug.Log("Can't select item");
            return;
        }

        ProjectileData data = selectedSlot.GetData();

        foreach (ProjectileData curData in PlayerRuntimeData.GetInstance().data.CookData.Recipe)
        {
            if (curData == data)
            {
                Debug.Log("Can't select item");
                return;
            }
        }

        selectedSlot.DecreaseCount();

        PlayerRuntimeData.GetInstance().data.CookData.Recipe.Add(data);
        m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].Sprite = data.inventorySprite;
        m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].Description = data.description;
    }
    #endregion

    #region Utility
    public int RecipeSize()
    {
        return PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count;
    }

    public void UpdateEquipedRecipeUI()
    {
        for (int i = 0; i < m_defaultRecipeSlots.Count; i++)
        {
            if (i < _equippedRecipe.Count)
            {
                m_defaultRecipeSlots[i].Sprite = _equippedRecipe[i].inventorySprite;
                m_defaultRecipeSlots[i].Color = Color.white;
            }
            else
            {
                m_defaultRecipeSlots[i].Sprite = null;
                m_defaultRecipeSlots[i].Color = Color.clear;
            }
        }
    }

    public bool IsDisplayed()
    {
        return gameObject.activeSelf;
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


