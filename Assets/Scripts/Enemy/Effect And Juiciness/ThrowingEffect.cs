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
        [SerializeField] private float maxLength;
        [SerializeField] private Vector3 direction;
<<<<<<< HEAD
        [SerializeField] private Transform target;
=======
>>>>>>> Enemy
        
        // Start is called before the first frame update
        void Start()
        {
            direction.Normalize();
        }

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

            while (timer < 1)
            {
                float x = xCurve.Evaluate(timer) * maxLength;
                float y = yCurve.Evaluate(timer) * maxHeight;
                
                transform.position = initPos + direction * x + Vector3.up * y;
                // transform.position = initPos + new Vector3(x, y, transform.position.z);
                
                timer += Time.deltaTime * speed;
                
                yield return new WaitForSeconds(Time.deltaTime * speed);
            }
        }
    }
}
