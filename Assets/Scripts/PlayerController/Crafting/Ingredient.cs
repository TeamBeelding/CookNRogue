using UnityEngine;

public class Ingredient : MonoBehaviour, IInteractable
{
    [SerializeField]
    ProjectileData _projectileData;

    public ProjectileData GetData
    {
        get => _projectileData;
    }

    public void Interactable(bool isInteractable)
    {
        //Nothing yet
    }
    public void Interact(string tag)
    {
        PlayerCookingInventory.Instance.AddIngredientToInventory(_projectileData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerCookingInventory.Instance.AddIngredientToInventory(_projectileData);
            Destroy(gameObject);
        }
    }
}
