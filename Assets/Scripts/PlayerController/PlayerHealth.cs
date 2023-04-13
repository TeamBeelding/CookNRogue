using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int _health;
    [SerializeField] int _maxHealth;
    [SerializeField] RectTransform _heart;
    [SerializeField] RectTransform _lifeContainer;
    List<Image> _heartsRenderer = new List<Image>();
    private void Start()
    {
        HealthInit();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }
    public void HealthInit()
    {
        _maxHealth = Mathf.Abs(_maxHealth);
        if (_maxHealth == 0)
            _maxHealth = 6;

        _health = _maxHealth;

        for (int i = 0; i < (_health / 2); i++)
        {
            RectTransform newHeart = Instantiate(_heart, _lifeContainer);
            _heartsRenderer.Add(newHeart.GetComponent<Image>());
        }

        UpdateHealthVisual();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        _health -= damage;
        UpdateHealthVisual();
    }

    public void Heal(int heal)
    {
        if (heal <= 0)
            return;

        _health += heal;
        UpdateHealthVisual();
    }

    public void UpdateHealthVisual()
    {
        for (int i = 1; i <= _heartsRenderer.Count;i++)
        {
            if ((i * 2) -1 <= (_health))
            {
                // FULL HEART
                _heartsRenderer[i-1].color = Color.white;

                if((i * 2) - 1 == _health)
                {
                    // HALF HEART
                    _heartsRenderer[i - 1].color = Color.grey;
                }
            }
            else
            {
                // NO HEART
                _heartsRenderer[i-1].color = Color.black;
            }
        }
    }

}
