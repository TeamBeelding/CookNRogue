using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BossController))]
public class ShockwaveController : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;

    private BossController bossController;
    private BossData data;
    private Coroutine shockwaveCoroutine;
    private float radius;

    private void Start()
    {
        bossController = GetComponent<BossController>();

        Reset();
    }

    private void Reset()
    {
        data = GetComponent<BossController>().GetBossDataRef();
    }

    public void StartShockwave()
    {
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

            ResetRadiusPos();
            //bossController.EndShockwave();
        }
    }

#if UNITY_EDITOR

    public void ResetRadiusPos()
    {
        if (shockwaveCoroutine != null)
            StopCoroutine(shockwaveCoroutine);
        
        radius = 0;
    }

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
        Gizmos.DrawSphere(transform.position, radius);

        if (data)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.GetMaxRadius);
        }
    }

#endif
}