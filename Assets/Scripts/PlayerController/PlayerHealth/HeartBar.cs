using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    [SerializeField] RectTransform _heart;
    List<Image> _heartsRenderer = new List<Image>();
    public static HeartBar instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    public void InitBar(int initialHealth)
    {
        for (int i = 0; i < (initialHealth / 2); i++)
        {
            RectTransform newHeart = Instantiate(_heart, transform); 
            _heartsRenderer.Add(newHeart.GetComponent<Image>());
        }
        UpdateHealthVisual(initialHealth);
    }

    public void UpdateHealthVisual(int health)
    {
        for (int i = 1; i <= _heartsRenderer.Count; i++)
        {
            if ((i * 2) - 1 <= (health))
            {
                // FULL HEART
                _heartsRenderer[i - 1].color = Color.white;

                if ((i * 2) - 1 == health)
                {
                    // HALF HEART
                    _heartsRenderer[i - 1].color = Color.grey;
                }
            }
            else
            {
                // NO HEART
                _heartsRenderer[i - 1].color = Color.black;
            }
        }
    }
}