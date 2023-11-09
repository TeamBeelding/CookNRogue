using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private static PoolManager instance;
    [SerializeField] private GameObject minimoyzPool;
    [SerializeField] private GameObject slimePool;
    [SerializeField] private GameObject TBHPool;
    [SerializeField] private GameObject LDSPool;
    [SerializeField] private GameObject CEPool;
    [SerializeField] private GameObject bulletPool;

    // reference to all queue
    // fonction to pull some obj

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static PoolManager Instance
    {
        get => instance;
    }

    #region Pooler object

    public GameObject InstantiateMinimoyz(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;

        return obj;
    }

    public GameObject InstantiateSlime(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;

        return obj;
    }

    public GameObject InstantiateTBH(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;

        return obj;
    }

    public GameObject InstantiateLDS(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;

        return obj;
    }

    public GameObject InstantiateCE(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;

        return obj;
    }

    public GameObject InstantiateBullet(Vector3 position, Quaternion quaternion)
    {
        GameObject gameObject = null;

        return gameObject;
    }

    #endregion
}
