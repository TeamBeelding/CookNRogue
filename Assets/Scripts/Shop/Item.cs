using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [SerializeReference] protected ItemData _data;
    private int _cost;
    private string _name;
    private string _description;
    [SerializeReference] GameObject _gfx;
    protected bool _hasTriggered = false;
    [SerializeField] private ParticleSystem _highlightPS;
    [SerializeField] private ParticleSystem _obtainPS;
    [SerializeField] AnimationCurve _scaleCurve;
    protected UnityEvent _triggerEffect = new UnityEvent();

    protected void Awake()
    {
        //_cost = _data.cost;
        _name = _data.name;
        _description = _data.description;
    }
    public void ShowItemGFX()
    {
        _gfx.SetActive(true);
    }

    protected virtual void Update()
    {
        //transform.LookAt(Camera.main.transform.position);
    }

    public void HideItemGFX()
    {
        _gfx.SetActive(false);
    }
    protected bool CanTrigger()
    {
        if (!_hasTriggered)
        {
            _hasTriggered = true;
            return true;
        }

        return false;
    }

    public void Interactable(bool isInteractable) { }

    public virtual bool AlreadyHasUpgrade()
    {
        return false;
    }


    public virtual void Interact(string tag)
    {
        bool hasEnoughCurrency = CurrencyManager.instance.CheckCurrency(_cost);

        if (!hasEnoughCurrency)
            return;

        CurrencyManager.instance.RemoveCurrency(_cost);
        Debug.Log("bought "+ name +" for "+  _cost + " currency!");
    }

    protected void ApplyItemRoutine(){ StartCoroutine(ToPlayer()); }

    protected IEnumerator ToPlayer()
    {
        Transform playerTransform = PlayerHealth.instance.transform;
        transform.parent = playerTransform;
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        float progress = 0;
        float scale = transform.localScale.x;
        while (distanceToPlayer > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, 0.05f);

            distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            transform.localScale = Vector3.one * (scale - _scaleCurve.Evaluate(progress));
            progress += Time.deltaTime;
            Mathf.Clamp01(progress);

            yield return new WaitForEndOfFrame();
        }

        if (_highlightPS)
            _highlightPS.Play();

        if (_obtainPS)
            _obtainPS.Play();

        _triggerEffect.Invoke();

        //TEMPORARY
        Destroy(gameObject);
    }
}
