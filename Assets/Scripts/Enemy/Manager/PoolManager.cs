using UnityEngine;

public enum PoolType
{
    MinimoyzVisual,
    Minimoyz,
    Slime,
    TBH,
    LDS,
    CE,
    Kamilkaze,
    Bullet
}

public class PoolManager : MonoBehaviour
{
    [SerializeField] private static PoolManager instance;
    [SerializeField] private GameObject minimoyzVisualPool;
    [SerializeField] private GameObject minimoyzPool;
    [SerializeField] private GameObject slimePool;
    [SerializeField] private GameObject TBHPool;
    [SerializeField] private GameObject LDSPool;
    [SerializeField] private GameObject CEPool;
    [SerializeField] private GameObject KamilkazePool;
    [SerializeField] private GameObject bulletPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            GameObject obj = InstantiateFromPool(PoolType.Kamilkaze, new Vector3(-184.521f, 0.5f, 22f), Quaternion.identity);
        }
    }

    public static PoolManager Instance
    {
        get => instance;
    }

    public GameObject InstantiateFromPool(PoolType pool, Vector3 position, Quaternion quaternion)
    {
        switch (pool)
        {
            case PoolType.MinimoyzVisual:
                return minimoyzVisualPool.GetComponent<IPooling>().Instantiating(position, quaternion);
            case PoolType.Minimoyz:
                return minimoyzPool.GetComponent<IPooling>().Instantiating(position, quaternion);
            case PoolType.Slime:
                return slimePool.GetComponent<IPooling>().Instantiating(position, quaternion);
            case PoolType.TBH:
                return TBHPool.GetComponent<IPooling>().Instantiating(position, quaternion);
            case PoolType.LDS:
                return LDSPool.GetComponent<IPooling>().Instantiating(position, quaternion);
            case PoolType.CE:
                return CEPool.GetComponent<IPooling>().Instantiating(position, quaternion);
            case PoolType.Kamilkaze:
                return KamilkazePool.GetComponent<IPooling>().Instantiating(position, quaternion);
            case PoolType.Bullet:
                return bulletPool.GetComponent<IPooling>().Instantiating(position, quaternion);
            default:
                Debug.LogWarning("Pool object not exist");
                return null;
        }
    }

    public void DesinstantiateFromPool(GameObject obj)
    {
        GameObject parent = obj.transform.parent.gameObject;
        parent.GetComponent<IPooling>().Desinstantiating(obj);
    }
}
