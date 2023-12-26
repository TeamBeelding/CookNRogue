using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public sealed class TotemInteraction : MonoBehaviour,IInteractable
{
    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Totem_Heal_Idle;
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Totem_Heal_Activation;

    [SerializeField] int _healPerIngredient = 1;

    [SerializeField] bool _playParticlesWhenHeal = true;
    [SerializeField] ParticleSystem _healPart;
    [SerializeField] ParticleSystemForceField _field;
    Transform _player;

    [SerializeField]
    GameObject m_buttonPrompt;

    private void Start()
    {
        _player = PlayerController.Instance.transform;
        _Play_SFX_Totem_Heal_Idle.Post(gameObject);
        m_buttonPrompt.SetActive(false);
    }
    public void Interact(string tag)
    {
        Regen();
    }

    public void Interactable(bool isInteractable)
    {
        m_buttonPrompt.SetActive(isInteractable);
    }

    public void Regen()
    {

        if (PlayerRuntimeData.GetInstance().data.BaseData.CurrentHealth >= PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth)
            return;

        if (PlayerCookingInventory.Instance.RemoveRandomIngredient())
        {
            _Play_SFX_Totem_Heal_Activation.Post(gameObject);
            PlayerHealth.instance.Heal(_healPerIngredient);

            if (_playParticlesWhenHeal)
            {
                StopAllCoroutines();
                StartCoroutine(PlayVFX());
            }
                
        }
        else
            return;
    }

    IEnumerator PlayVFX()
    {
        _healPart.Play();

        float timer = 0;

        while(timer < 2)
        {
            _field.transform.position = _player.position;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopAllCoroutines();
            StartCoroutine(PlayVFX());
        }
    }
#endif

}
