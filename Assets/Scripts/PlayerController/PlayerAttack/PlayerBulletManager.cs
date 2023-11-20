using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerBulletManager : MonoBehaviour
{
    public static PlayerBulletManager instance;
    [SerializeField] Transform _bulletContainer;
    List<PlayerBulletBehaviour> _bulletList = new List<PlayerBulletBehaviour>();
    [SerializeField] GameObject _playerBulletPrefab;
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
        _bulletList.Clear();
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < _initialListSize; i++)
        {
            GameObject temp = Instantiate(_playerBulletPrefab, transform.position, Quaternion.identity, _bulletContainer);
            PlayerBulletBehaviour playerBullet = temp.GetComponent<PlayerBulletBehaviour>();
            _bulletList.Add(playerBullet);
            temp.SetActive(false);
        }
    }

    public PlayerBulletBehaviour GetAvailableBullet()
    {
        foreach (PlayerBulletBehaviour bullet in _bulletList)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }

        GameObject temp = Instantiate(_playerBulletPrefab, transform.position, Quaternion.identity, _bulletContainer);
        PlayerBulletBehaviour playerBullet = temp.GetComponent<PlayerBulletBehaviour>();

        _bulletList.Add(playerBullet);
        return playerBullet;
    }
}
