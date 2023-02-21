using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.Build;


public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [SerializeField]
    private GameObject[] EasyLevels;
    [SerializeField]
    private GameObject[] HardLevels;

    [HideInInspector]
    public string[] LevelNames;

    [SerializeField]
    private NavMeshSurface navMeshSurface;
    [SerializeField]
    private bool loadSurface = false;

    [SerializeField]
    private EnemyManager EnemyManagerScript;  

    public bool isHard;

    GameObject CurrentLevel;

    [SerializeField]
    GameObject Player;
    
    public Transform SpawnPoint;

    [SerializeField]
    TransitionController Transition;

    public event Action OnRoomStart;

    void Awake()
    {
        InitLevelNames();

        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        LoadRandomLevel();

        navMeshSurface.BuildNavMesh();
    }

    private void Update()
    {
        if (loadSurface) 
        {
            Debug.Log("It has been built");
            navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
            navMeshSurface.BuildNavMesh();
            loadSurface = false;
        }
    }

    public void LoadRandomLevel()
    {

        TransitionToLevel();

        if (!isHard)
        {
            LoadLevel(EasyLevels);
        }
        else
        {
            LoadLevel(HardLevels);
        }
    }

    public void LoadPecifiedLevel(int i)
    {

        TransitionToLevel();

        if (i < 3)
        {
            isHard = false;
            CurrentLevel = Instantiate(EasyLevels[i], Vector3.zero, Quaternion.identity);
            loadSurface = true;
        }
        else
        {
            isHard = true;
            CurrentLevel = Instantiate(HardLevels[i - EasyLevels.Length], Vector3.zero, Quaternion.identity);
            loadSurface = true;
        }
    }

    private void TransitionToLevel()
    {
        EnemyManagerScript.DestroyAll();

        Transition.LoadTransition();

        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel);
        }
    }

    private void LoadLevel(GameObject[] levels) 
    {
        int rand = UnityEngine.Random.Range(0, levels.Length);
        CurrentLevel = Instantiate(levels[rand], Vector3.zero, Quaternion.identity);

        if (OnRoomStart!= null)
        {
            OnRoomStart();
        }
        Player.transform.position = SpawnPoint.position;
        loadSurface = true;
    }

    private void OnEnable()
    {
        InitLevelNames();
    }

    public void InitLevelNames()
    {
        LevelNames = new string[EasyLevels.Length + HardLevels.Length];

        for (int i = 0; i < EasyLevels.Length; i++)
        {
            LevelNames[i] = EasyLevels[i].name;
        }

        for (int i = EasyLevels.Length; i < EasyLevels.Length + HardLevels.Length; i++)
        {
            LevelNames[i] = HardLevels[i - EasyLevels.Length].name;
        }
    }

}

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor 
{
    private int selected = 0;

    public override void OnInspectorGUI() 
    {

        DrawDefaultInspector();
        RoomManager room = (RoomManager)target;


        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("TOOLS: ", "");
        GuiLine(1);

        selected = EditorGUILayout.Popup("Specified Level", selected, room.LevelNames);

        if (GUILayout.Button("Load Specified Level"))
        {
            room.LoadPecifiedLevel(selected);
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Load Random Level"))
        {
            room.LoadRandomLevel();
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