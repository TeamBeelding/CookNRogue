using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int _health;
    [SerializeField] int _maxHealth;
    [SerializeField] private AK.Wwise.Event _Play_SFX_Health_Collect;
    [SerializeField] private AK.Wwise.Event _Play_MC_Hit;
    [SerializeField] private AK.Wwise.Event _Play_MC_Death;
    [SerializeField] private AK.Wwise.Event _Play_Reset_Ingredient;
    HeartBar _heartBar;
    private void Start()
    {
        _heartBar = HeartBar.instance;
        HealthInit();
    }
    public void HealthInit()
    {
        //GUARDS
        _maxHealth = Mathf.Abs(_maxHealth);
        if (_maxHealth == 0)
            _maxHealth = 6;

        if(_maxHealth%2 !=0)
            _maxHealth++;
        
        

        _health = _maxHealth;
        _heartBar.InitBar(_health);

    }

    /*
     * return: true if player is alive after taking damage
     */
    public bool TakeDamage(int damage)
    {
        if (damage <= 0) return true;

        _health -= damage;
        _heartBar.UpdateHealthVisual(_health);
        _Play_MC_Hit.Post(gameObject);
        _Play_SFX_Health_Collect.Post(gameObject);

        if (_health <= 0)
        {
            _health = _maxHealth;
            _heartBar.UpdateHealthVisual(_health);
            _Play_MC_Death.Post(gameObject);
            _Play_Reset_Ingredient.Post(gameObject);

            // RoomManager.instance.RestartLevel();
            return false;
        }

        return true;
    }

    public void Heal(int heal)
    {
        if (heal <= 0)
            return;

        _health += heal;

        _heartBar.UpdateHealthVisual(_health);
    }

    private void Reset()
    {
        _maxHealth = 6;
    }

}
