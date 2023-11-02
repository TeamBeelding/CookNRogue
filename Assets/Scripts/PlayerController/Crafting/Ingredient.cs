using UnityEngine;

public class Ingredient : MonoBehaviour, IInteractable
{
    [SerializeField]
    ProjectileData _projectileData;
    [SerializeField]
    ParticleSystem _particles;
    public ProjectileData GetData
    {
        get => _projectileData;
    }

    void Start()
    {
        _particles = transform.GetChild(2).GetComponentInChildren<ParticleSystem>();
        _particles.transform.parent = null;
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
            OnPickUpIngredientParticles();
            Destroy(gameObject);
        }
    }

    private void OnPickUpIngredientParticles()
    {
        ParticleSystem.MainModule main = _particles.main;
        main.simulationSpeed = 4;

        var emission = _particles.emission;
        emission.enabled = false;

        var vol = _particles.velocityOverLifetime; ;
        vol.y = 1.5f;

        var noise = _particles.noise;
        noise.strengthX = 2;
        noise.strengthY = 2;
        noise.strengthZ = 2;

        Destroy(transform.parent,1);
    }

    
}
