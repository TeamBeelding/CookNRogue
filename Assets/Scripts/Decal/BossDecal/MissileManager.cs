using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MissileManager : MonoBehaviour
{
    public static MissileManager instance;
    [SerializeField] Transform BossMissileContainer;
    List<MissileBoss> MissileList = new List<MissileBoss>();
    [SerializeField] GameObject MissileDecalPrefab;
    [SerializeField] int _initialListSize;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MissileList.Clear();
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < _initialListSize; i++)
        {
            GameObject temp = Instantiate(MissileDecalPrefab, transform.position, Quaternion.identity, BossMissileContainer);
            MissileBoss missile = temp.GetComponent<MissileBoss>();
            MissileList.Add(missile);
            temp.SetActive(false);
        }
    }

    public MissileBoss GetAvailableMissile()
    {
        foreach (MissileBoss missileBoss in MissileList)
        {
            if (!missileBoss.gameObject.activeInHierarchy)
            {
                missileBoss.gameObject.SetActive(true);
                return missileBoss;
            }
        }

        GameObject temp = Instantiate(MissileDecalPrefab, transform.position, Quaternion.identity, BossMissileContainer);
        MissileBoss missile = temp.GetComponent<MissileBoss>();

        MissileList.Add(missile);
        return missile;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
