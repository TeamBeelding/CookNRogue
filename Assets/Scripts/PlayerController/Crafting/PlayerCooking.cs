using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController), typeof(PlayerAttack))]
public class PlayerCooking : MonoBehaviour
{
    #region Variables

    [SerializeField, Tooltip("Cooking time for a 1 ingredient bullet")]
    float m_1IngredientCookTime;

    [SerializeField, Tooltip("Cooking time for a 2 ingredients bullet")]
    float m_2IngredientCookTime;

    [SerializeField, Tooltip("Cooking time for a 3 ingredients bullet")]
    float m_3IngredientCookTime;

    [SerializeField, Tooltip("Random delay to spawn the QTE based on the total cook time,\r\n" +
        "Where 0 is the start of cooking and 1 is the end,\r\n" +
        "And X is the lowest possible generated value and Y the highest.")]
    Vector2 m_QTESpawnDelay;

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
    bool _spawnedQTE;

    float _totalCookTime;
    float _randQTEDelay;

    Coroutine _craftingRoutine;

    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Cook;

    #endregion

    private void Reset()
    {
        m_attackScript = m_attackScript != null ? m_attackScript : GetComponent<PlayerAttack>();
        m_QTEScript = m_QTEScript != null ? m_QTEScript : GetComponent<PlayerCookingQTE>();

        m_1IngredientCookTime = 4;
        m_2IngredientCookTime = 6;
        m_3IngredientCookTime = 8;

        m_QTESpawnDelay = new Vector2(0.4f, 0.6f);
    }

    void Start()
    {
        m_attackScript = m_attackScript != null ? m_attackScript : GetComponent<PlayerAttack>();
        m_QTEScript = m_QTEScript != null ? m_QTEScript : GetComponent<PlayerCookingQTE>();

        _inventoryScript = PlayerCookingInventory.Instance;
        _playerController = PlayerController.Instance;
        //animator = GetComponentInChildren<Animator>();
        m_cookingProgressVisuals.SetActive(false);
    }

    #region Cooking
    public void StartCooking()
    {
        if (!_craftingInProgress)
        {
            _inventoryScript.Show(true);
            Debug.Log("true");
        }
        else
        {
            RestartCrafting();
        }
    }

    public void StopCooking()
    {
        if (!_craftingInProgress)
        {
            if (_inventoryScript.IsDisplayed())
            {
                _inventoryScript.Show(false);
                Debug.Log("false");
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
                data.audioState.SetValue();
            }

            _inventoryScript.Show(false);

            //Set cook time
            switch (bulletIngredientsNb)
            {
                case 1:
                    {
                        _totalCookTime = m_1IngredientCookTime;
                        break;
                    }

                case 2:
                    {
                        _totalCookTime = m_2IngredientCookTime;
                        break;
                    }

                case 3:
                    {
                        _totalCookTime = m_3IngredientCookTime;
                        break;
                    }
            }

            //Set QTE delay
            _randQTEDelay = Random.Range(m_QTESpawnDelay.x, m_QTESpawnDelay.y);

            //Start coroutine
            _curProgressTime = 0f;
            _craftingRoutine = StartCoroutine(ICraftingLoop(_randQTEDelay * _totalCookTime));

            //Show UI
            m_cookingProgressVisuals.SetActive(true);

            //Stop Slow Motion
            Time.timeScale = 1f;
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
        _playerController.StopCookingState();
        m_attackScript.OnAmmunitionChange();

        //reset
        _craftingRoutine = null;
        _craftingInProgress = false;
        _spawnedQTE = false;

        //Hide UI
        m_cookingProgressVisuals.SetActive(false);
        
        _playerController.CheckingIfCookingIsDone();
    }

    IEnumerator ICraftingLoop(float delay)
    {
        //Check crafting time
        while (_curProgressTime < _totalCookTime)
        {
            //Check QTE spawn time
            if (!_spawnedQTE && _curProgressTime >= delay)
            {
                
                SpawnQTE();
            }
            //UI
            m_cookingProgressBarFill.fillAmount = _curProgressTime / _totalCookTime;

            //Advance Time
            _curProgressTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        CompleteCrafting();
    }
    #endregion

    #region QTE
    void SpawnQTE()
    {
        _playerController.QTEAppear();
        
        m_QTEScript.StartQTE();
        _spawnedQTE = true;
    }

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
            
            _playerController.CheckingIfCookingIsDone();
        }
    }
    #endregion
}
