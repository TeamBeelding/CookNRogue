using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> queue;

    private void Awake()
    {
        AddAllToQueue();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddAllToQueue()
    {
        foreach (GameObject obj in transform.GetComponentsInChildren<GameObject>())
            queue.Enqueue(obj);
    }

    public void EnqueueObject()
    {
        if (queue.Count > 0)
            queue.Dequeue();
    }
}
