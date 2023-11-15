using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MissileDecalManager : MonoBehaviour
{
    public static MissileDecalManager instance;
    List<MissileDecal> DecalList = new List<MissileDecal>();
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
        DecalList.Clear();
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < _initialListSize; i++)
        {
            GameObject temp = Instantiate(MissileDecalPrefab, transform.position, Quaternion.identity, transform);
            MissileDecal missileDecal = temp.GetComponent<MissileDecal>();
            DecalList.Add(missileDecal);
            temp.SetActive(false);
        }
    }

    public MissileDecal GetAvailableDecal()
    {
        foreach (MissileDecal decal in DecalList)
        {
            if (!decal.gameObject.activeInHierarchy)
            {
                decal.gameObject.SetActive(true);
                return decal;
            }
        }

        GameObject temp = Instantiate(MissileDecalPrefab, transform.position, Quaternion.identity, transform);
        MissileDecal missileDecal = temp.GetComponent<MissileDecal>();

        DecalList.Add(missileDecal);
        return missileDecal;
    }
}
