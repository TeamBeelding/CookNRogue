using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour, IPooling
{
    [SerializeField] private Queue<GameObject> queue;

    private int remainingElement;

    private void Awake()
    {
        queue = new Queue<GameObject>();

        foreach (GameObject obj in transform.GetComponentsInChildren<GameObject>())
        {
            queue.Enqueue(obj);
            obj.SetActive(false);
        }

        remainingElement = queue.Count;
    }

    public void DequeueObject()
    {
        throw new System.NotImplementedException();
    }

    public void QueueObject()
    {
        throw new System.NotImplementedException();
    }

    public GameObject Instantiating(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;

        if (remainingElement == 0)
        {
            // Queue new element
            obj = Instantiate(obj, position, quaternion);
            obj.transform.parent = gameObject.transform;
        }

        remainingElement--;

        return obj;
    }

    public void Desinstantiating(GameObject obj)
    {
        obj.SetActive(false);

        remainingElement++;
    }
}
