using UnityEngine;

namespace Enemy.Slime
{
    public class ThrowObject : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float speed;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void Throw()
        {
            curve.Evaluate(speed);
        }
    }
}