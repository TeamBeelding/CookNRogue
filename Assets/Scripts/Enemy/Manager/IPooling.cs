using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooling
{
    public void QueueObject();
    public void DequeueObject();
}
