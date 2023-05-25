using System;
using System.Collections;
using UnityEngine;

namespace Enemy.Effect_And_Juiciness
{
    public class ThrowingEffect : MonoBehaviour
    {
        [SerializeField] private AnimationCurve xCurve;
        [SerializeField] private AnimationCurve yCurve;

        private Transform _target;
        private float _maxHeight = 4;
        private float _maxLength = 10;
        private float _speed = 2;

        private void Start()
        {
            _target = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                ThrowMinimoyz(_target, _maxHeight, _speed);
        }

        public void ThrowMinimoyz(Transform target, float maxHeight, float speed)
        {
            
            _target = target;
            _speed = speed;
            _maxHeight = maxHeight;
            
            StartCoroutine(ICurve());
        }
        
        private IEnumerator ICurve()
        {
            float timer = 0;
            Vector3 initPos = transform.position;
            Vector3 direction = (_target.position - transform.position).normalized;
            _maxLength = Vector3.Distance(transform.position, _target.position);

            while (timer < 1)
            {
                float x = xCurve.Evaluate(timer) * _maxLength;
                float y = yCurve.Evaluate(timer) * _maxHeight;
                
                transform.position = initPos + direction * x + Vector3.up * y;
                
                timer += Time.deltaTime * _speed;
                yield return new WaitForSeconds(Time.deltaTime * _speed);
            }
        }
    }
}
