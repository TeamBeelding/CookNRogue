using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCookingInventory : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_UI_Cook_Hover;
    [SerializeField]
    private AK.Wwise.Event _Play_UI_Cook_Select;
    [SerializeField]
    private AK.Wwise.Event _Play_UI_Cook_Cancel;


    #region Variables
    [SerializeField]
    Vector3 m_cameraOffset;

    [SerializeField]
    AnimationCurve m_showAnimPosCurve;

    [SerializeField]
    Image m_transition;

    [SerializeField] 
    List<PlayerCookingInventoryWheel> m_inventoryWheels;

    public PlayerCookingInventoryWheel GetWheel()
    {
        return m_inventoryWheels[0];
    }

    [SerializeField]
    List<GameObject> m_recipeCounterSlots;

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
    CameraController _cameraController;
    PlayerCooking _cookingScript;

    bool _areControlsLocked;
    Vector3 _shownPosition;
    Vector3 _hiddenPosition;
    Vector3 _initScale;
    Vector3 _shownCamOffset;
    Vector3 _hiddenCamOffset;
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
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);

        /*_shownPosition = m_UIHolder.localPosition;
        _hiddenPosition = _shownPosition;
        _hiddenPosition.y -= gameObject.GetComponent<RectTransform>().rect.height;*/
        //m_UIHolder.localPosition = _hiddenPosition;

        _cameraController = CameraController.instance;
        _shownCamOffset = _cameraController.OffsetCoord + m_cameraOffset;
        _hiddenCamOffset = _cameraController.OffsetCoord;

        _initScale = m_UIHolder.localScale;
        m_UIHolder.localScale = Vector3.zero;

        //Set Recipe Slots
        PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb = 1;
        m_recipeCounterSlots[1].SetActive(false);
        m_recipeCounterSlots[2].SetActive(false);
        m_defaultRecipeSlots[1].gameObject.SetActive(false);
        m_defaultRecipeSlots[2].gameObject.SetActive(false);

        _cookingScript = PlayerCooking.Instance;

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
        //Vector3 targetPos = _shownPosition;
        //Vector3 initPos = m_UIHolder.localPosition;

        Vector3 initCameraOffset = _cameraController.OffsetCoord;
        Vector3 targetCameraOffset = _shownCamOffset;


        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            //m_UIHolder.localPosition = Vector3.Lerp(initPos, targetPos, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.zero, _initScale, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            _cameraController.OffsetCoord = Vector3.Lerp(initCameraOffset, targetCameraOffset, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            //Time.timeScale = Mathf.Lerp(1, 0, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Color tempc = m_transition.color;
            tempc.a = Mathf.Lerp(0, 0.9f, m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_transition.color = tempc;
            yield return null;
        }

        //m_UIHolder.localPosition = targetPos;
        m_UIHolder.localScale = _initScale;
        _cameraController.OffsetCoord = targetCameraOffset;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
    }

    IEnumerator HideAnimation()
    {
        _areControlsLocked = true;

        float time = PlayerRuntimeData.GetInstance().data.CookData.ShowAnimDuration;
        //Vector3 initPos = m_UIHolder.localPosition;
        //Vector3 targetPos = _hiddenPosition;

        Vector3 initCameraOffset = _cameraController.OffsetCoord;
        Vector3 targetCameraOffset = _hiddenCamOffset;

        for (float f = _curAnimProgress > 0 ? (1 - _curAnimProgress) * time : 0; f < time; f += Time.unscaledDeltaTime)
        {
            _curAnimProgress = f / time;
            //m_UIHolder.localPosition = Vector3.Lerp(targetPos, initPos, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_UIHolder.localScale = Vector3.Lerp(Vector3.zero, _initScale, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            _cameraController.OffsetCoord = Vector3.Lerp(targetCameraOffset, initCameraOffset, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            //Time.timeScale = Mathf.Lerp(1, 0, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            Color tempc = m_transition.color;
            tempc.a = Mathf.Lerp(0, 0.9f, 1 - m_showAnimPosCurve.Evaluate(_curAnimProgress));
            m_transition.color = tempc;
            yield return null;
        }

        //m_UIHolder.localPosition = targetPos;
        m_UIHolder.localScale = Vector3.zero;
        _cameraController.OffsetCoord = targetCameraOffset;
        _areControlsLocked = false;
        _curAnimProgress = 0;
        _curShowRoutine = null;
        gameObject.SetActive(false);
        m_defaultUI.SetActive(true);

        ResetSlotsVisuals();
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
        AmmunitionBar.instance.ResetAmmoBar(true);
        _equippedRecipe.Clear();
        //Fuse ingredients's effects and stats
        float averageDmg = 0;
        PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown = 0f;
        PlayerRuntimeData.GetInstance().data.AttackData.Ammunition = 0;
        foreach (ProjectileData ingredient in PlayerRuntimeData.GetInstance().data.CookData.Recipe)
        {
            PlayerRuntimeData.GetInstance().data.AttackData.AttackSize += ingredient._size;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed += ingredient._speed;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag += ingredient._drag;
            PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown += ingredient._attackDelay;
            PlayerRuntimeData.GetInstance().data.AttackData.Ammunition += ingredient._ammunition;
            averageDmg += ingredient._damage;

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
                PlayerRuntimeData.GetInstance().data.AttackData.Ammunition *= 1f;
                break;
            case 2:
                averageDmg *= _damageFactor[1];
                PlayerRuntimeData.GetInstance().data.AttackData.Ammunition *= 0.8f;
                break;
            case 3:
                averageDmg *= _damageFactor[2];
                PlayerRuntimeData.GetInstance().data.AttackData.Ammunition *= 0.7f;
                break;
        }

        //LARGE CAULDRON CHECK
        if (PlayerRuntimeData.GetInstance().data.InventoryData.LargeCauldron)
            PlayerRuntimeData.GetInstance().data.AttackData.Ammunition += PlayerRuntimeData.GetInstance().data.InventoryData.LargeCauldronValue;

        //BIG SPATULE CHECK
        if (PlayerRuntimeData.GetInstance().data.InventoryData.BigSpatule && PlayerRuntimeData.GetInstance().data.CookData.QTESuccess)
            PlayerRuntimeData.GetInstance().data.AttackData.Ammunition += PlayerRuntimeData.GetInstance().data.InventoryData.BigSpatuleValue;
        
        PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage = averageDmg;

        //WODDEN SPOON CHECK
        if (PlayerRuntimeData.GetInstance().data.InventoryData.WoodenSpoon)
            PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage += PlayerRuntimeData.GetInstance().data.InventoryData.WoodenSpoonValue;

        Debug.Log(PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown);
        //Average rate of fire
        PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown /= PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count;
        Debug.Log(PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown);

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
            /*if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count < PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb)
            {
                ProjectileData slotData = slot.GetData();
                m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Sprite = slotData.inventorySprite;
                m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Color = Color.white;
                m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Description = slotData.description;
            }*/

            slot.Highlight(true);
            _currentSlot = slot;
            _Play_UI_Cook_Hover.Post(gameObject);
        }
    }

    public void OnSelectorInputStop()
    {
        if (_areControlsLocked)
        {
            return;
        }

        //Item Description
        /*if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count < PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb)
        {
            m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Sprite = null;
            m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Color = Color.clear;
            m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count].Description = null;
        }*/

        if(_currentSlot != null)
        {
            _currentSlot.Highlight(false);
            _currentSlot = null;
        }

        m_navigateKey.SetActive(true);
        m_selectKey.SetActive(false);
        _Play_UI_Cook_Hover.Post(gameObject);
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
        _Play_UI_Cook_Select.Post(gameObject);
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
        ProjectileData data = selectedSlot.GetData();

        foreach (ProjectileData curData in PlayerRuntimeData.GetInstance().data.CookData.Recipe)
        {
            if (curData == data)
            {
                selectedSlot.IncreaseCount();
                selectedSlot.Selected(false);
                m_recipeCounterSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].SetActive(true);
                PlayerRuntimeData.GetInstance().data.CookData.Recipe.Remove(data);

                return;
            }
        }

        if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count >= PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb || !selectedSlot.CanSelect())
        {
            Debug.Log("Can't select item");
            return;
        }

        selectedSlot.DecreaseCount();
        selectedSlot.Selected(true);

        PlayerRuntimeData.GetInstance().data.CookData.Recipe.Add(data);
        m_recipeCounterSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].SetActive(false);
        /*m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].Sprite = data.inventorySprite;
        m_recipeSlots[PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count - 1].Description = data.description;*/

        if (PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count == PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb)
        {
            _cookingScript.StartCrafting();
        }
    }
    #endregion

    #region Utility
    public int RecipeSize()
    {
        return PlayerRuntimeData.GetInstance().data.CookData.Recipe.Count;
    }

    public void IncreaseMaxRecipeSlots()
    {
        if(PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb >= 3)
        {
            return;
        }

        int index = PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb;

        m_recipeCounterSlots[index].SetActive(true);
        m_defaultRecipeSlots[index].gameObject.SetActive(true);

        PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb += 1;
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

    public void ResetSlotsVisuals()
    {
        for (int i = 0; i < m_inventoryWheels[_currentWheelIndex].SlotsNumber(); i++)
        {
            m_inventoryWheels[_currentWheelIndex].GetSlot(i).ResetVisuals();
        }

        for(int i = 0; i < PlayerRuntimeData.GetInstance().data.CookData.RecipeMaxIngredientNb; i++)
        {
            m_recipeCounterSlots[i].SetActive(true);
        }
    }

    public void ResetRecipeUI()
    {
        /*foreach (PlayerCookingRecipeSlot slot in m_recipeSlots)
        {
            slot.Sprite = null;
            slot.Color = new(1, 1, 1, 0); 
            slot.Description = null;
        }*/

        m_navigateKey.SetActive(true);
        m_selectKey.SetActive(false);
        m_craftKey.SetActive(false);
    }

    public bool RemoveRandomIngredient()
    {
        //WE GET ALL OF THE SLOTS THAT HAVE AT LEAST ONE INGREDIENT
        List<PlayerCookingInventorySlot> availableSlots = new List<PlayerCookingInventorySlot>();

        foreach (PlayerCookingInventoryWheel wheel in m_inventoryWheels)
        {
            for (int i = 0; i < 8; i++)
            {
                PlayerCookingInventorySlot slot = wheel.GetSlot(i);
                if (slot._ingredientCount > 0)
                    availableSlots.Add(slot);
            }
        }

        if (availableSlots.Count == 0)
            return false;

        int randomSlot = Random.Range(0, availableSlots.Count);
        availableSlots[randomSlot].DecreaseCount();
        return true;

    }
    #endregion

    [System.Serializable]
    public class PlayerCookingInventoryWheel
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


