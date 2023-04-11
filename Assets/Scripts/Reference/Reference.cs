using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISet<T>
{
    void Set(T value);
}

public class Reference<T> : ScriptableObject, ISet<T>
{
    private T _instance;
    
    public T Instance { get => _instance; }

    public void Set(T value)
    {
        _instance = value;
    }
}
