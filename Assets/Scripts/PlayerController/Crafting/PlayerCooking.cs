using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController), typeof(PlayerAttack))]
public class PlayerCooking : MonoBehaviour
{
    #region Variables

    [SerializeField]
    PlayerAttack m_attackScript;

    [SerializeField]
    PlayerCookingQTE m_QTEScript;

    [SerializeField]
    GameObject m_cookingProgressVisuals;

    [SerializeField]
    Image m_cookingProgressBarFill;

    PlayerCookingInventory _inventoryScript;
    PlayerController _playerController;

    bool _craftingInProgress;
    float _curProgressTime;

    float _totalCookTime;
    float _randQTEDelay;

    Material _fillMaterial;

    Coroutine _craftingRoutine;

    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Cook;
    [SerializeField]
    private AK.Wwise.Event _Play_Reset_Ingredient;

    static PlayerCooking _instance;
    public static PlayerCooking Instance
    {
        get => _instance;
    }

    #endregion

    private void Reset()
    {
        m_attackScript = m_attackScript != null ? m_attackScript : GetComponent<PlayerAttack>();
        m_QTEScript = m_QTEScript != null ? m_QTEScript : GetComponent<PlayerCookingQTE>();

        PlayerRuntimeData.GetInstance().data.CookData.OneIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultOneIngredientCookTime;
        PlayerRuntimeData.GetInstance().data.CookData.TwoIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultTwoIngredientCookTime;
        PlayerRuntimeData.GetInstance().data.CookData.ThreeIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultThreeIngredientCookTime;
        PlayerRuntimeData.GetInstance().data.CookData.ThreeIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultThreeIngredientCookTime;
        PlayerRuntimeData.GetInstance().data.CookData.QteSpawnDelay = PlayerRuntimeData.GetInstance().data.CookData.DefaultQteSpawnDelay;
    }

    private void Awake()
    {
        //Set instance
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    void Start()
    {
        m_attackScript = m_attackScript != null ? m_attackScript : GetComponent<PlayerAttack>();
        m_QTEScript = m_QTEScript != null ? m_QTEScript : GetComponent<PlayerCookingQTE>();

        _inventoryScript = PlayerCookingInventory.Instance;
        _playerController = PlayerController.Instance;
        //animator = GetComponentInChildren<Animator>();
        _fillMaterial = m_cookingProgressBarFill.material;
        m_cookingProgressVisuals.SetActive(false);

        PlayerRuntimeData.GetInstance().data.CookData.OneIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultOneIngredientCookTime;
        PlayerRuntimeData.GetInstance().data.CookData.TwoIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultTwoIngredientCookTime;
        PlayerRuntimeData.GetInstance().data.CookData.ThreeIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultThreeIngredientCookTime;
        PlayerRuntimeData.GetInstance().data.CookData.QteSpawnDelay = PlayerRuntimeData.GetInstance().data.CookData.DefaultQteSpawnDelay;
    }

    #region Cooking
    public void StartCooking()
    {
        if (!_craftingInProgress)
        {
            PlayerRuntimeData.GetInstance().data.CookData.QTESuccess = false;
            _inventoryScript.Show(true);
        }
        else
        {
            RestartCrafting();
        }
    }
    
    public bool GetCraftingInProgress()
    {
        return _craftingInProgress;
    }

    public void StopCooking()
    {
        if (!_craftingInProgress)
        {
            if (_inventoryScript.IsDisplayed())
            {
                _inventoryScript.Show(false);
                _inventoryScript.CancelCraft();
            }
        }
        else
        {
            PauseCrafting();
        }
    }
    #endregion

    #region Crafting
    public void StartCrafting()
    {
        if (!_craftingInProgress)
        {
            //Recipe Check
            float bulletIngredientsNb = _inventoryScript.RecipeSize();
            if (bulletIngredientsNb > 3 || bulletIngredientsNb < 1)
            {
                return;
            }

            //Start crafting
            _craftingInProgress = true;
            _Play_SFX_Cook.Post(gameObject);

            //Reset last bullet parametters
            m_attackScript.ResetParameters();
            foreach(ProjectileData data in _inventoryScript.EquippedRecipe)
            {
                _Play_Reset_Ingredient.Post(gameObject);
            }

            _inventoryScript.Show(false);

            //Set cook time
            switch (bulletIngredientsNb)
            {
                case 1:
                    {
                        _totalCookTime = PlayerRuntimeData.GetInstance().data.CookData.OneIngredientCookTime;
                        break;
                    }

                case 2:
                    {
                        _totalCookTime = PlayerRuntimeData.GetInstance().data.CookData.TwoIngredientCookTime;
                        break;
                    }

                case 3:
                    {
                        _totalCookTime = PlayerRuntimeData.GetInstance().data.CookData.ThreeIngredientCookTime;
                        break;
                    }
            }

            //Set QTE delay
            _randQTEDelay = Random.Range(PlayerRuntimeData.GetInstance().data.CookData.QteSpawnDelay.x, PlayerRuntimeData.GetInstance().data.CookData.QteSpawnDelay.y);

            //Start coroutine
            _curProgressTime = 0f;
            _craftingRoutine = StartCoroutine(ICraftingLoop(_randQTEDelay * _totalCookTime));

            //Show UI
            m_cookingProgressVisuals.SetActive(true);

            //Stop Slow Motion
            //Time.timeScale = 1f;
        }
    }

    public void PauseCrafting()
    {
        m_QTEScript.FailQTE();

        //Stop coroutine
        StopCoroutine(_craftingRoutine);
        _craftingRoutine = null;

        //Hide UI
        m_cookingProgressVisuals.SetActive(false);
    }

    public void RestartCrafting()
    {
        //Restart coroutine
        _craftingRoutine = StartCoroutine(ICraftingLoop(_randQTEDelay * _totalCookTime));

        //Show UI
        m_cookingProgressVisuals.SetActive(true);

        _Play_SFX_Cook.Post(gameObject);
    }

    void CompleteCrafting()
    {
        _inventoryScript.CraftBullet();
        m_attackScript.OnAmmunitionChange();

        //reset
        _craftingRoutine = null;
        _craftingInProgress = false;

        //Hide UI
        m_cookingProgressVisuals.SetActive(false);
        
        _playerController.CheckingIfCookingIsDone();
        _playerController.StopCookingState();
        //_playerController._ignoreCook = true;
    }

    IEnumerator ICraftingLoop(float delay)
    {
        m_QTEScript.StartQTE(delay);

        //Check crafting time
        while (_curProgressTime < _totalCookTime)
        {
            //UI
            _fillMaterial.SetFloat("_FillAmount", _curProgressTime / _totalCookTime);

            //Advance Time
            _curProgressTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        m_QTEScript.ResetQTE();

        _fillMaterial.SetFloat("_FillAmount", 0f);
        CompleteCrafting();
    }
    #endregion

    #region QTE
    public void CheckQTE()
    {
        m_QTEScript.CheckQTE();
    }

    public void CompletedQTE()
    {
        //Complete crafting instantly
        if (_craftingRoutine != null)
        {
            StopCoroutine(_craftingRoutine);
            CompleteCrafting();
            _inventoryScript.IncreaseMaxRecipeSlots();
            
            _playerController.CheckingIfCookingIsDone();
        }
    }
    #endregion

    #region Utilities

    public void Clear()
    {
        _inventoryScript.Clear();
    }

    #endregion
}
