using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour, IPooling
{
    [SerializeField] private int remainingElement;
    [SerializeField] private GameObject objectToPool;

    private Queue<GameObject> queue;

    private void Awake()
    {
        queue = new Queue<GameObject>();

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.transform.transform.SetParent(transform, false);
            queue.Enqueue(obj);
            obj.SetActive(false);
            remainingElement++;
        }
    }

    public GameObject Instantiating(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;

        if (remainingElement == 0)
        {
            obj = Instantiate(objectToPool, position, quaternion);
            obj.transform.parent = gameObject.transform;

            return obj;
        }

        obj = queue.Dequeue();
        obj.SetActive(true);

        obj.transform.localPosition = position;
        obj.transform.localRotation = quaternion;

        remainingElement--;

        return obj;
    }

    public void Desinstantiating(GameObject obj)
    {
        queue.Enqueue(obj);
        obj.SetActive(false);

        remainingElement++;
    }
}
