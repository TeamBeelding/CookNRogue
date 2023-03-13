using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "LevelData", menuName = "Level")]
public class LevelListData : ScriptableObject
{
    [Header("Statistics")]
    [SerializeField]
    private int amountOfRooms = 10;

    [SerializeField]
    [Range(0, 10)]
    private int EasyLevels = 6;

    [SerializeField]
    [Range(0, 10)]
    private int HardLevels = 4;

    [SerializeField]
    private List<LevelData> _levelList;

    public List<LevelData> LevelList
    {
        get => _levelList;
        set => LevelList = value;
    }

    //public int GetProbability(int index)
    //{
    //    return _levelList[index].Prob;
    //}

    //public GameObject GetLevel(int index)
    //{
    //    return _levelList[index].Level;
    //}
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelListData))]
public class LevelListDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("TOOLS: ", "");
        GuiLine(1);

        if (GUILayout.Button("Shake"))
        {
            CameraController.instance._shake = true;
        }

        if (GUILayout.Button("Zoom"))
        {
            CameraController.instance._zoom = true;
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
