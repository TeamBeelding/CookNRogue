using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _ingredients;
    [SerializeField] float _ingredientScale;

    [SerializeField] Transform[] m_spawnPoints;

    List<GameObject> _pool = new();

    private void Start()
    {
        _pool.AddRange(_ingredients);
        Invoke(nameof(SpawnIngredient), 0.2f);
    }

    private void SpawnIngredient()
    {   
        for(int i = 0; i < m_spawnPoints.Length; i++)
        {
            int rand = Random.Range(0, _pool.Count);
            GameObject ingredient = Instantiate(_pool[rand], m_spawnPoints[i].position, Quaternion.identity);
            ingredient.transform.localScale = Vector3.one * _ingredientScale;
            RoomManager.instance.AddIngredient(ingredient);

            _pool.RemoveAt(rand);
        }
    }
}
