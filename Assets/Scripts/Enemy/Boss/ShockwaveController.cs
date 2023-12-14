using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(BossController))]
public class ShockwaveController : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;

    private BossData data;
    private Coroutine shockwaveCoroutine;
    private float radius;
    [SerializeField] Transform VFXContainer;
    ParticleSystem[] _shockwaveParts;
    [SerializeField] LayerMask _shockwaveLayerMask;
    [SerializeField] DecalProjector _decalProjector;
    [SerializeField] float _fadeSpeed = 1;
    [SerializeField,Range(0,1)] float _targetAlpha = 1;


    [Header("Sound")]
    [SerializeField]
    private AK.Wwise.Event _Play_SFX_Boss_Erupt;

    private void Start()
    {
        _shockwaveParts = VFXContainer.GetComponentsInChildren<ParticleSystem>();

        var mat = _decalProjector.material;
        mat.SetFloat("_Alpha", 0);

        _decalProjector.size = new Vector3(data.radius * 2, data.radius * 2,1);
    }
    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        data = GetComponent<BossController>().GetBossDataRef();
    }

    public void StartShockwave()
    {
        StartCoroutine(DecalFadeIn());
        _Play_SFX_Boss_Erupt.Post(gameObject);
        foreach (ParticleSystem particle in _shockwaveParts)
            particle.Play();

        bool hasHittingPlayer = false;
        float duration = data.GetShockwaveDuration;

        shockwaveCoroutine = StartCoroutine(InterpolateOverTime());

        IEnumerator InterpolateOverTime()
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float curveValue = animationCurve.Evaluate(elapsedTime / duration);

                radius = Mathf.Lerp(0, data.GetMaxRadius, curveValue);

                foreach(ParticleSystem particle in _shockwaveParts)
                {
                    var shape = particle.shape;
                    shape.radius = radius;
                } 

                if (!hasHittingPlayer)
                {
                    Collider[] hits = Physics.OverlapSphere(transform.position, radius, _shockwaveLayerMask);

                    foreach (Collider c in hits)
                    {
                        if (c.GetComponent<PlayerController>())
                        {
                            PlayerController.Instance.TakeDamage(data.GetShockwaveDamage);

                            hasHittingPlayer = true;
                            break;
                        }
                    }
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            foreach (ParticleSystem particle in _shockwaveParts)
                particle.Stop();

            StartCoroutine(DecalFadeOut());
        }
       
    }

    public void ResetRadiusPos()
    {
        if (shockwaveCoroutine != null)
            StopCoroutine(shockwaveCoroutine);
        radius = 0;
    }

    public IEnumerator DecalFadeIn()
    {
        var mat = _decalProjector.material;
        
        mat.SetFloat("_Alpha", 0);

        //FADE
        float alphaProgress = 0;
        while (alphaProgress < _targetAlpha)
        {
            alphaProgress += Time.fixedDeltaTime * _fadeSpeed * _targetAlpha;
            mat.SetFloat("_Alpha", alphaProgress);
            yield return new WaitForFixedUpdate();
        }
        mat.SetFloat("_Alpha", _targetAlpha);
    }
    public IEnumerator DecalFadeOut()
    {
        var mat = _decalProjector.material;

        mat.SetFloat("_Alpha", 0);

        //FADE
        float alphaProgress = 1;
        while (alphaProgress > 0)
        {
            alphaProgress -= Time.fixedDeltaTime * _fadeSpeed;
            mat.SetFloat("_Alpha", alphaProgress);
            yield return new WaitForFixedUpdate();
        }
        mat.SetFloat("_Alpha", 0);
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Shockwave button"))
        {
            Debug.Log("Button clicked");
            StartShockwave();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);

        if (data)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.GetMaxRadius);
        }
    }

#endif
}