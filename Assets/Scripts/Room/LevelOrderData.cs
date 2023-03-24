using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "LevelOrderList", menuName = "Level/LevelOrder")]
public class LevelOrderData : ScriptableObject
{
    [Header("Statistics")]

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

    private string[] _levelNames;
    public string[] LevelNames
    {
        get => _levelNames;
    }

    private string[] _roomTypes;
    public string[] RoomTypes
    {
        get => _roomTypes;
    }

    private void OnEnable()
    {
        _roomTypes = new string[] { "Corridor", "Room", "Shop", "Final Room" };
        InitLevelNames();
    }

    public void InitLevelNames()
    {
        _levelNames = new string[LevelLists.CorridorList.Length + LevelLists.RoomList.Length + LevelLists.ShopList.Length + LevelLists.FinalList.Length];

        for (int i = 0; i < LevelLists.CorridorList.Length; i++)
        {
            _levelNames[i] = LevelLists.CorridorList[i].name;
        }

        for (int i = LevelLists.CorridorList.Length; i < LevelLists.CorridorList.Length + LevelLists.RoomList.Length; i++)
        {
            _levelNames[i] = LevelLists.RoomList[i - LevelLists.CorridorList.Length].name;
        }

        for (int i = LevelLists.CorridorList.Length + LevelLists.RoomList.Length; i < LevelLists.CorridorList.Length + LevelLists.RoomList.Length + LevelLists.ShopList.Length; i++)
        {
            _levelNames[i] = LevelLists.ShopList[i - LevelLists.CorridorList.Length - LevelLists.RoomList.Length].name;
        }

        for (int i = LevelLists.FinalList.Length + LevelLists.CorridorList.Length + LevelLists.RoomList.Length; i < LevelLists.CorridorList.Length + LevelLists.RoomList.Length + LevelLists.ShopList.Length + LevelLists.FinalList.Length; i++)
        {
            _levelNames[i] = LevelLists.FinalList[i - LevelLists.CorridorList.Length - LevelLists.RoomList.Length - LevelLists.ShopList.Length].name;
        }
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

//#if UNITY_EDITOR
//[CustomEditor(typeof(LeveOrdertData))]
//public class LeveOrdertDataEditor : Editor
//{
//    private int selected = 0;

//    public override void OnInspectorGUI()
//    {

//        DrawDefaultInspector();
//        LeveOrdertData order = (LeveOrdertData)target;


//        EditorGUILayout.Separator();

//        EditorGUILayout.LabelField("TOOLS: ", "");
//        GuiLine(1);

//        selected = EditorGUILayout.Popup("Specified Level", selected, order.LevelData.CorridorList.name);

//        if (GUILayout.Button("Load Specified Level"))
//        {
//            room.LoadPecifiedLevel(selected);
//        }

//        EditorGUILayout.Separator();

//        if (GUILayout.Button("Load Random Level"))
//        {
//            room.LoadRandomLevel();
//        }
//    }

//    void GuiLine(int i_height = 1)
//    {
//        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
//        rect.height = i_height;
//        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
//        EditorGUILayout.Separator();
//    }
//}
//#endif

//#if UNITY_EDITOR
//[CustomEditor(typeof(LevelListData))]
//public class LevelListDataEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        LevelListData _levelListData = new LevelListData();

//        LevelData data = null;

//        //data = (LevelData)EditorGUILayout.ObjectField(data, typeof(LevelData), true);

//        if (GUILayout.Button("Update"))
//        {
//            for (int i = 0; i < _levelListData.m_amountOfRooms; i++)
//            {
//                data = (LevelData)EditorGUILayout.ObjectField(data, typeof(LevelData), true);
//            }
//        }


//        if (data)
//        {

//        }

//        //EditorGUILayout.Separator();

//        //EditorGUILayout.LabelField("TOOLS: ", "");
//        //GuiLine(1);

//        //if (GUILayout.Button("Shake"))
//        //{
//        //    CameraController.instance._shake = true;
//        //}

//        //if (GUILayout.Button("Zoom"))
//        //{
//        //    CameraController.instance._zoom = true;
//        //}
//    }


//    //
//    // Summary:
//    //     Checks if this editor requires constant repaints in its current state.
//    //public override bool RequiresConstantRepaint() = true;

//    //public override void OnInspectorUpdate() 
//    //{ 

//    //}

//    void GuiLine(int i_height = 1)
//    {
//        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
//        rect.height = i_height;
//        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
//        EditorGUILayout.Separator();
//    }
//}
//#endif
