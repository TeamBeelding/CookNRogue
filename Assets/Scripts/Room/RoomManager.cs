using System;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEditor;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [SerializeField]
    private LevelOrderData m_Levels;

    public LevelOrderData Levels 
    { 
        get => m_Levels;
    }

    [SerializeField]
    private int m_amountOfLevels;
    [SerializeField]
    private string m_currentLevelType;
    [SerializeField]
    private int m_currentLevelIndex = -1;


    [SerializeField]
    private NavMeshSurface m_navMeshSurface;
    [SerializeField]
    private bool m_loadSurface = false;

    [SerializeField]
    private EnemyManager m_enemyManagerScript;  

    public bool isHard;

    private GameObject _currentLevel;

    [SerializeField]
    private GameObject m_player;
    
    public Transform SpawnPoint;

    [SerializeField]
    private TransitionController m_transition;

    public event Action OnRoomStart;

    void Awake()
    {
        //InitLevelNames();

        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        RestartLevel();

        m_navMeshSurface.BuildNavMesh();

        m_amountOfLevels = m_Levels.OrderList.Count;
    }

    private void Update()
    {
        if (m_loadSurface) 
        {
            m_navMeshSurface.UpdateNavMesh(m_navMeshSurface.navMeshData);
            m_navMeshSurface.BuildNavMesh();
            m_loadSurface = false;
        }
    }

    public void LoadNextLevel()
    {
        if (m_currentLevelIndex >= m_amountOfLevels - 1)
        {
            RestartLevel();
        }
        else
        {
            TransitionToLevel();
            m_currentLevelIndex += 1;
            m_currentLevelType = m_Levels.OrderList[m_currentLevelIndex];
            PickFromType(m_currentLevelType);
        }
    }

    public void RestartLevel()
    {
        TransitionToLevel();

        m_currentLevelIndex = 0;
        m_currentLevelType = m_Levels.OrderList[m_currentLevelIndex];
        PickFromType(m_currentLevelType);
    }

    private void PickFromType(string currentLevelType) 
    {
        switch (currentLevelType)
        {
            case "Room":
                LoadLevel(m_Levels.LevelLists.RoomList);
                break;
            case "Corridor":
                LoadLevel(m_Levels.LevelLists.CorridorList);
                break;
            case "Shop":
                LoadLevel(m_Levels.LevelLists.ShopList);
                break;
            case "Final":
                LoadLevel(m_Levels.LevelLists.FinalList);
                break;
        }
    }
    public void PickFromTypeAndIndex(string currentLevelType, int index)
    {
        TransitionToLevel();
        m_currentLevelType = currentLevelType;

        switch (currentLevelType)
        {
            case "Room":
                _currentLevel = Instantiate(m_Levels.LevelLists.RoomList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Corridor":
                _currentLevel = Instantiate(m_Levels.LevelLists.CorridorList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Shop":
                _currentLevel = Instantiate(m_Levels.LevelLists.ShopList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Final":
                _currentLevel = Instantiate(m_Levels.LevelLists.FinalList[index], Vector3.zero, Quaternion.identity);
                break;
        }
        OnRoomStart?.Invoke();
        m_player.transform.position = SpawnPoint.position;
        m_loadSurface = true;
    }

    private void TransitionToLevel()
    {
        m_enemyManagerScript.DestroyAll();

        m_transition.LoadTransition();

        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
    }

    private void LoadLevel(GameObject[] levels) 
    {
        int rand = UnityEngine.Random.Range(0, levels.Length);
        _currentLevel = Instantiate(levels[rand], Vector3.zero, Quaternion.identity);

        OnRoomStart?.Invoke();
        m_player.transform.position = SpawnPoint.position;
        m_loadSurface = true;
    }

    private void Reset()
    {
        
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor 
{
    private int selected = 0;
    private int typeSelected = 0;

    public override void OnInspectorGUI() 
    {

        DrawDefaultInspector();
        RoomManager room = (RoomManager)target;


        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("TOOLS: ", "");
        GuiLine(1);

        if (GUILayout.Button(" Load Next Level -> "))
        {
            if (Application.isPlaying)
            {
                room.LoadNextLevel();
            }
        }

        EditorGUILayout.Separator();

        typeSelected = EditorGUILayout.Popup("Level Type", typeSelected, room.Levels.RoomTypes);

        Debug.Log(typeSelected);

        switch (room.Levels.RoomTypes[typeSelected])
        {
            case "Room":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[0]);
                break;
            case "Corridor":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[1]);
                break;
            case "Shop":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[2]);
                break;
            case "Final":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[3]);
                break;
        }

        Debug.Log(room.Levels.RoomTypes[typeSelected]);

        if (GUILayout.Button("Load Specified Level"))
        {
            if (Application.isPlaying)
            {
                room.PickFromTypeAndIndex(room.Levels.RoomTypes[typeSelected], selected);
            }
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