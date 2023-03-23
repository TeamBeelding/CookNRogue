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
    private GameObject[] _corridorList;
    public GameObject[] CorridorList
    {
        get => _corridorList;
    }

    [SerializeField]
    private GameObject[] _roomList;
    public GameObject[] RoomList
    {
        get => _roomList;
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

//#if UNITY_EDITOR
//[CustomEditor(typeof(LevelListData))]
//public class LevelListDataEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        LevelListData _levelListData = new LevelListData();

//        LevelData data = null;

//        data = (LevelData)EditorGUILayout.ObjectField(data, typeof(LevelData), true);

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
