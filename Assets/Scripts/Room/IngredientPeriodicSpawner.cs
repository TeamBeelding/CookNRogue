using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IngredientPeriodicSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Ingredients;

    [SerializeField] private float _timeBtwSpawn;
    private float _timer;
    private Transform _currentIngredient;
    [SerializeField] Transform _spawnPoint;

    private void Start()
    {
        _timer = _timeBtwSpawn;
    }

    public void Update()
    {
        if (_currentIngredient)
            return;

        if (_timer > 0)
            _timer -= Time.deltaTime;
        else
        {
            _timer += _timeBtwSpawn;
            //SPAWN INGREDIENT
            _currentIngredient = SpawnIngredient();
            _currentIngredient.localScale = Vector3.one;
        }
    }

    public Transform SpawnIngredient()
    {
        return Instantiate(Ingredients[Random.Range(0, Ingredients.Length)], _spawnPoint.position, Quaternion.identity).transform;
    }
}
