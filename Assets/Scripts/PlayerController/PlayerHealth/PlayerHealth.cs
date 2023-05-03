using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    int _health;
    [SerializeField] int _maxHealth;
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

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        _health -= damage;
        _heartBar.UpdateHealthVisual(_health);
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
