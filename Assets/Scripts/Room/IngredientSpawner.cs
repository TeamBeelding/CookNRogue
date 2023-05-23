using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _ingredients;

    private void Start()
    {
        Invoke("SpawnIngredient", 0.2f);
    }

    private void SpawnIngredient()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 3);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * 3), Color.red, 999);
        if (hit.collider != null)
        {
            transform.position = hit.collider.transform.position + (Vector3.up * 0.75f);
            int rand = Random.Range(0, _ingredients.Length - 1);
            GameObject ingredient = Instantiate(_ingredients[rand], transform.position, Quaternion.identity);
        }
        
    }
}
