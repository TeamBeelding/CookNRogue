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


    // ===================================================================

    [SerializeField]
    private GameObject[] _roomEasyList;
    public GameObject[] RoomEasyList
    {
        get => _roomEasyList;
    }

    [SerializeField]
    private GameObject[] _roomNormalList;
    public GameObject[] RoomNormalList
    {
        get => _roomNormalList;
    }

    [SerializeField]
    private GameObject[] _roomHardList;
    public GameObject[] RoomHardList
    {
        get => _roomHardList;
    }

    // ===================================================================


    [SerializeField]
    private GameObject[] _corridorEasyList;
    public GameObject[] CorridorEasyList
    {
        get => _corridorEasyList;
    }

    [SerializeField]
    private GameObject[] _corridorNormalList;
    public GameObject[] CorridorNormalList
    {
        get => _corridorNormalList;
    }

    [SerializeField]
    private GameObject[] _corridorHardList;
    public GameObject[] CorridorHardList
    {
        get => _corridorHardList;
    }

    // ===================================================================

    [SerializeField]
    private GameObject[] _shopList;
    public GameObject[] ShopList
    {
        get => _shopList;
    }

    // ===================================================================


    [SerializeField]
    private GameObject[] _finalEasyList;
    public GameObject[] FinalEasyList
    {
        get => _finalEasyList;
    }

    [SerializeField]
    private GameObject[] _finalNormalList;
    public GameObject[] FinalNormalList
    {
        get => _finalNormalList;
    }

    [SerializeField]
    private GameObject[] _finalHardList;
    public GameObject[] FinalHardList
    {
        get => _finalHardList;
    }

}
