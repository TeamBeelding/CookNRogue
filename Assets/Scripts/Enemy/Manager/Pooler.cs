using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour, IPooling
{
    [SerializeField] private int poolSize = 5;
    [SerializeField] private int remainingElement;
    [SerializeField] private GameObject objectToPool;

    private Queue<GameObject> queue;

    private void Start()
    {
        queue = new Queue<GameObject>();

        foreach (Transform t in transform)
        {
            queue.Enqueue(t.gameObject);
            remainingElement++;
        }

        StartCoroutine(IWaitingForQueue());

        IEnumerator IWaitingForQueue()
        {
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(objectToPool);
                obj.SetActive(false);
                obj.transform.transform.SetParent(transform, false);
                queue.Enqueue(obj);
                remainingElement++;
            }
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

        //PlaceItCorrectly(obj);

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

    private void PlaceItCorrectly(GameObject obj)
    {
        obj.SetActive(true);
        obj.SetActive(false);
    }
}
