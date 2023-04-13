using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;



[CreateAssetMenu(fileName = "LevelListData", menuName = "Level/LevelList")]
public class LevelListData : ScriptableObject
{
    [Header("All Levels")]

    [SerializeField]
    private GameObject[] _hubList;
    public GameObject[] HubList
    {
        get => _hubList;
    }

    [SerializeField]
    private GameObject[] _roomList;
    public GameObject[] RoomList
    {
        get => _roomList;
    }

    [SerializeField]
    private GameObject[] _corridorList;
    public GameObject[] CorridorList
    {
        get => _corridorList;
    }

    [SerializeField]
    private GameObject[] _shopList;
    public GameObject[] ShopList
    {
        get => _shopList;
    }

    [SerializeField]
    private GameObject[] _finalList;
    public GameObject[] FinalList
    {
        get => _finalList;
    }

}
