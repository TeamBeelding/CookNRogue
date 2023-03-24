using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;



[CreateAssetMenu(fileName = "LevelListData", menuName = "Level/LevelList")]
public class LevelListData : ScriptableObject
{
    [Header("Statistics")]

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
        get => _roomList;
    }

    [SerializeField]
    private GameObject[] _finalList;
    public GameObject[] FinalList
    {
        get => _roomList;
    }
}
