using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [SerializeField] private AK.Wwise.Event _Play_SFX_Health_Collect;
    [SerializeField] private AK.Wwise.Event _Play_MC_Hit;
    [SerializeField] private AK.Wwise.Event _Play_MC_Death;

    HeartBar _heartBar;

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
            PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth = PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth;
            _heartBar.UpdateHealthVisual(PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth);
            _Play_MC_Death.Post(gameObject);

            // RoomManager.instance.RestartLevel();
            return false;
        }

        return true;
    }

    public void UpgradeMaxHealth(int additionalHealth)
    {
        PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth += additionalHealth;
        Heal(additionalHealth);
    }

    public void Heal(int heal)
    {
        if (heal <= 0)
            return;
        
        PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth += heal;

        if (PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth > PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth)
            PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth = PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth;

        _heartBar.UpdateHealthVisual(PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth);
    }

    private void Reset()
    {
        PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth = PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth;
    }

}
