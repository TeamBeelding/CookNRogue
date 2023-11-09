using UnityEngine;

public interface IPooling
{
    public GameObject Instantiating(Vector3 position, Quaternion quaternion);
    public void Desinstantiating(GameObject obj);
}