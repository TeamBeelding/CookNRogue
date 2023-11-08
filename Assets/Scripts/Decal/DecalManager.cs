using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour
{

    public static DecalManager instance;
    List<BulletDecal> DecalList = new List<BulletDecal>();
    [SerializeField] GameObject DecalPrefab;

    [SerializeField] int _initialListSize;
    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        
    }

    private void Start()
    {
        DecalList.Clear();
        Init();
    }

    public void Init()
    {
        for(int i = 0; i < _initialListSize; i++)
        {
            GameObject temp = Instantiate(DecalPrefab, transform.position, Quaternion.identity, transform);
            BulletDecal bulletDecal = temp.GetComponent<BulletDecal>();
            DecalList.Add(bulletDecal);
            temp.SetActive(false);
        }
    }

    public BulletDecal GetAvailableDecal()
    {
        foreach (BulletDecal decal in DecalList)
        {
            if (!decal.gameObject.activeInHierarchy)
            {
                decal.gameObject.SetActive(true);
                return decal;
            }
        }

        GameObject temp = Instantiate(DecalPrefab, transform.position, Quaternion.identity,transform);
        BulletDecal bulletDecal = temp.GetComponent<BulletDecal>();

        DecalList.Add(bulletDecal);
        return bulletDecal;
    }
}
