using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _ingredients;
    [SerializeField] float _ingredientScale;

    private void Start()
    {
        Invoke("SpawnIngredient", 0.2f);
    }

    private void SpawnIngredient()
    {   
        int rand = Random.Range(0, _ingredients.Length);
        GameObject ingredient = Instantiate(_ingredients[rand], transform.position, Quaternion.identity);
        ingredient.transform.localScale = Vector3.one * _ingredientScale;
        RoomManager.instance.AddIngredient(ingredient);
    }
}
