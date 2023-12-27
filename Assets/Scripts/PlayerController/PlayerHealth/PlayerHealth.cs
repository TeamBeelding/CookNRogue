using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [SerializeField] private AK.Wwise.Event _Play_SFX_Health_Collect;
    [SerializeField] private AK.Wwise.Event _Play_MC_Hit;
    [SerializeField] private AK.Wwise.Event _Play_MC_Death;

    HeartBar _heartBar;

    [Header("Hit Time Scale")]
    [SerializeField] float _targetTimeScale;
    [SerializeField] float _scalingDuration;
    [SerializeField] AnimationCurve _scaleCurve;

    [SerializeField] Color _hitColor = Color.red;
    [SerializeField] SkinnedMeshRenderer _playerMesh;
    Material playerMaterial;
    Color _baseColor;

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(this);

        instance = this;
    }
    private void Start()
    {
        _heartBar = HeartBar.instance;
        HealthInit();

        playerMaterial = _playerMesh.material;
        _baseColor = playerMaterial.GetColor("_BaseColor");
        playerMaterial.SetColor("_BaseColor", _baseColor);
    }
    public void HealthInit()
    {
        //GUARDS
        PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth = Mathf.Abs(PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth);

        if (PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth == 0)
            PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth = 6;

        if(PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth%2 !=0)
            PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth++;
        
        

        PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth = PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth;
        PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth = PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth;
        _heartBar.InitBar(PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth);
    }

    /*
     * return: true if player is alive after taking damage
     */
    public bool TakeDamage(int damage)
    {
        if (damage <= 0) return true;

        PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth -= damage;
        _heartBar.UpdateHealthVisual(PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth);
        _Play_MC_Hit.Post(gameObject);
        _Play_SFX_Health_Collect.Post(gameObject);

        if (PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth <= 0)
        {
            PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth = 0;
            _heartBar.UpdateHealthVisual(PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth);
            _Play_MC_Death.Post(gameObject);

            // RoomManager.instance.RestartLevel();
            return false;
        }
        else
        {
            StartCoroutine(DamageSlowTime());
        }

        return true;
    }

    public void UpgradeMaxHealth(int additionalHealth)
    {
        for(int i = 0; i < additionalHealth; i++)
            _heartBar.AddHeart();

        PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth += (additionalHealth*2);
        Heal(additionalHealth *2);
    }

    public void Heal(int heal)
    {
        if (heal <= 0)
            return;
        
        PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth += heal;

        if (PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth > PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth)
            PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth = PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth;

        _heartBar.UpdateHealthVisual(PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth);
        _heartBar.PlayHeartBarAnimation();
    }

    private void Reset()
    {
        PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth = PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth;
    }

    IEnumerator DamageSlowTime()
    {       
        /*if (playerMaterial)
            baseColor = playerMaterial.GetColor("_BaseColor");*/
        

        //SCALE
        float scaleProgress = 0;
        float timer = 0f;
        while (scaleProgress < 1)
        {
            if (playerMaterial)
            {
                Color curColor = Color.Lerp(Color.white, _hitColor, 1 - scaleProgress);
                playerMaterial.SetColor("_BaseColor", curColor);
            }
          
            float newScale = Mathf.Lerp(1, _targetTimeScale, _scaleCurve.Evaluate(scaleProgress));
            Time.timeScale = newScale;
            timer += Time.fixedUnscaledDeltaTime;
            scaleProgress = timer / _scalingDuration;
            //Debug.Log(scaleProgress);
            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
        }


        if (playerMaterial)
            playerMaterial.SetColor("_BaseColor", _baseColor);   
        Time.timeScale = 1f;
    }
}
