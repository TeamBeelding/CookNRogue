using UnityEngine;

public class Ingredient : MonoBehaviour, IInteractable
{
    [SerializeField]
    ProjectileData _projectileData;
    [SerializeField]
    ParticleSystem _particles;

    [Header("Particle behaviour")]
    [SerializeField] float _simulationSpeed = 4;
    [SerializeField] float _yVelocity = 1.5f;
    [SerializeField] float _noiseValue = 4;

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
        main.simulationSpeed = _simulationSpeed;

        var emission = _particles.emission;
        emission.enabled = false;

        var vol = _particles.velocityOverLifetime; ;
        vol.y = _yVelocity;

        var noise = _particles.noise;
        noise.strengthX = _noiseValue;
        noise.strengthY = _noiseValue;
        noise.strengthZ = _noiseValue;

        Destroy(transform.parent,1);
    }

    
    
}
