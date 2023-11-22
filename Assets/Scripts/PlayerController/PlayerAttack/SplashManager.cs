using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    public static SplashManager instance;
    [SerializeField] Transform _splashContainer;
    List<Splash> _splashList = new List<Splash>();
    [SerializeField] GameObject _splashPrefab;
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
        _splashList.Clear();
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < _initialListSize; i++)
        {
            GameObject temp = Instantiate(_splashPrefab, transform.position, Quaternion.identity, _splashContainer);
            Splash playerBullet = temp.GetComponent<Splash>();
            _splashList.Add(playerBullet);
            temp.SetActive(false);
        }
    }

    public Splash GetAvailableSplash()
    {
        foreach (Splash bullet in _splashList)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }

        GameObject temp = Instantiate(_splashPrefab, transform.position, Quaternion.identity, _splashContainer);
        Splash playerBullet = temp.GetComponent<Splash>();

        _splashList.Add(playerBullet);
        return playerBullet;
    }
}
