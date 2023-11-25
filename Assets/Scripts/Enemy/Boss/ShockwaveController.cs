using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BossController))]
public class ShockwaveController : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;

    private BossData data;
    private Coroutine shockwaveCoroutine;
    private float radius;
    [SerializeField] ParticleSystem _shockwavePart;

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
        if (_shockwavePart != null)
            _shockwavePart.Play();

        var shape = _shockwavePart.shape;

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

                if(_shockwavePart != null) 
                    shape.radius = radius;

                if (!hasHittingPlayer)
                {
                    Collider[] hits = Physics.OverlapSphere(transform.position, radius);

                    foreach (Collider c in hits)
                    {
                        if (c.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            PlayerController.Instance.TakeDamage(data.GetShockwaveDamage);

                            hasHittingPlayer = true;
                        }
                    }
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            if (_shockwavePart != null)
                _shockwavePart.Stop();
        }
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