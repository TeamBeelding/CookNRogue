using System.Collections;
using UnityEngine;

namespace Enemy.Effect_And_Juiciness
{
    public class ThrowingEffect : MonoBehaviour
    {
        [SerializeField] private AnimationCurve xCurve;
        [SerializeField] private AnimationCurve yCurve;
        [SerializeField] private float speed;
        [SerializeField] private float maxHeight;
        [SerializeField] private Transform target;
        
        private float _maxLength;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(ICurve());
            }
        }

        private IEnumerator ICurve()
        {
            float timer = 0;
            Vector3 initPos = transform.position;
            Vector3 direction = (target.position - transform.position).normalized;
            _maxLength = Vector3.Distance(transform.position, target.position);

            while (timer < 1)
            {
                float x = xCurve.Evaluate(timer) * _maxLength;
                float y = yCurve.Evaluate(timer) * maxHeight;
                
                transform.position = initPos + direction * x + Vector3.up * y;

                timer += Time.deltaTime * speed;
                
                yield return new WaitForSeconds(Time.deltaTime * speed);
            }
        }
    }
}
