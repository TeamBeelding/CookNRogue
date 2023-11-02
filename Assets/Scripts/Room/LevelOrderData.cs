using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelOrderList", menuName = "Level/LevelOrder")]
public class LevelOrderData : ScriptableObject
{
    [Header("Order")]

    [SerializeField]
    private LevelListData m_LevelLists;
    public LevelListData LevelLists
    {
        get => m_LevelLists;
    }

    [SerializeField]
    private List<string> m_orderList;
    public List<string> OrderList
    {
        get => m_orderList;
    }

    [SerializeField]
    private LevelOrderData m_PrevLevelOrder;
    public LevelOrderData PrevLevelOrder
    {
        get => m_PrevLevelOrder;
    }

    [SerializeField]
    private LevelOrderData m_NextLevelOrder;
    public LevelOrderData NextLevelOrder
    {
        get => m_NextLevelOrder;
    }

    // Name
    private string[] _hubNames;

    private string[] _roomEasyNames;
    private string[] _roomNormalNames;
    private string[] _roomHardNames;

    private string[] _corridorEasyNames;
    private string[] _corridorNormalNames;
    private string[] _corridorHardNames;

    private string[] _shopNames;

    private string[] _finalEasyNames;
    private string[] _finalNormalNames;
    private string[] _finalHardNames;

    // list of strings

    private List<string[]> _levelNames;
    public List<string[]> LevelNames 
    {
        get => _levelNames;
    }

    // Types
    private string[] _roomTypes;
    public string[] RoomTypes
    {
        get => _roomTypes;
    }

    private void OnEnable()
    {
        _levelNames = new List<string[]>();
        _roomTypes = new string[] { "Hub", "Easy_Room", "Normal_Room", "Hard_Room", "Easy_Corridor", "Normal_Corridor", "Hard_Corridor", "Shop", "Easy_Final", "Normal_Final", "Hard_Final" };
        InitLevelNames();
    }

    public void InitLevelNames()
    {

        _hubNames = new string[LevelLists.HubList.Length];

        _roomEasyNames = new string[LevelLists.RoomEasyList.Length];
        _roomNormalNames = new string[LevelLists.RoomNormalList.Length];
        _roomHardNames = new string[LevelLists.RoomHardList.Length];

        _corridorEasyNames = new string[LevelLists.CorridorEasyList.Length];
        _corridorNormalNames = new string[LevelLists.CorridorNormalList.Length];
        _corridorHardNames = new string[LevelLists.CorridorHardList.Length];

        _shopNames = new string[LevelLists.ShopList.Length];

        _finalEasyNames = new string[LevelLists.FinalEasyList.Length];
        _finalNormalNames = new string[LevelLists.FinalNormalList.Length];
        _finalHardNames = new string[LevelLists.FinalHardList.Length];

        // ===============================================================

        for (int i = 0; i < LevelLists.HubList.Length; i++)
        {
            _hubNames[i] = LevelLists.HubList[i].name;
        }
        _levelNames.Add(_hubNames);

        // ===============================================================

        for (int i = 0; i < LevelLists.RoomEasyList.Length; i++)
        {
            _roomEasyNames[i] = LevelLists.RoomEasyList[i].name;
        }
        _levelNames.Add(_roomEasyNames);

        for (int i = 0; i < LevelLists.RoomNormalList.Length; i++)
        {
            _roomNormalNames[i] = LevelLists.RoomNormalList[i].name;
        }
        _levelNames.Add(_roomNormalNames);

        for (int i = 0; i < LevelLists.RoomHardList.Length; i++)
        {
            _roomHardNames[i] = LevelLists.RoomHardList[i].name;
        }
        _levelNames.Add(_roomHardNames);

        // ===============================================================

        for (int i = 0; i < LevelLists.CorridorEasyList.Length; i++)
        {
            _corridorEasyNames[i] = LevelLists.CorridorEasyList[i].name;
        }
        _levelNames.Add(_corridorEasyNames);

        for (int i = 0; i < LevelLists.CorridorNormalList.Length; i++)
        {
            _corridorNormalNames[i] = LevelLists.CorridorNormalList[i].name;
        }
        _levelNames.Add(_corridorNormalNames);

        for (int i = 0; i < LevelLists.CorridorHardList.Length; i++)
        {
            _corridorHardNames[i] = LevelLists.CorridorHardList[i].name;
        }
        _levelNames.Add(_corridorHardNames);

        // ===============================================================

        for (int i = 0; i < LevelLists.ShopList.Length; i++)
        {
            _shopNames[i] = LevelLists.ShopList[i].name;
        }
        _levelNames.Add(_shopNames);

        // ===============================================================

        for (int i = 0; i < LevelLists.FinalEasyList.Length; i++)
        {
            _finalEasyNames[i] = LevelLists.FinalEasyList[i].name;
        }
        _levelNames.Add(_finalEasyNames);

        for (int i = 0; i < LevelLists.FinalNormalList.Length; i++)
        {
            _finalNormalNames[i] = LevelLists.FinalNormalList[i].name;
        }
        _levelNames.Add(_finalNormalNames);

        for (int i = 0; i < LevelLists.FinalHardList.Length; i++)
        {
            _finalHardNames[i] = LevelLists.FinalHardList[i].name;
        }
        _levelNames.Add(_finalHardNames);

        // ===============================================================
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelOrderData))]
public class LeveOrdertDataEditor : Editor
{
    private int selected = 0;

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        LevelOrderData order = (LevelOrderData)target;

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("TOOLS: ", "");
        GuiLine(1);

        selected = EditorGUILayout.Popup("Specified Level", selected, order.RoomTypes);

        if (GUILayout.Button("Add Room Type"))
        {
            order.OrderList.Add(order.RoomTypes[selected]);
        }
    }

    void GuiLine(int i_height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Separator();
    }
}
#endif