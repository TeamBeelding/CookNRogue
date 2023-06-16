using System;
using Enemy;
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
    private PlayerController m_player;

    public Transform m_spawnPoint;

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
        m_player = m_player != null ? m_player : PlayerController.Instance;
        //m_player = GameObject.FindGameObjectWithTag("Player");
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
            if (m_Levels.NextLevelOrder == null)
            {
                m_player.EndGame();
                return;
            }

            m_Levels = m_Levels.NextLevelOrder;
            RemoveIngredientsFromRoom();
            TransitionToLevel();
            m_currentLevelIndex = 0;
            m_currentLevelType = m_Levels.OrderList[m_currentLevelIndex];
            PickFromType(m_currentLevelType);
        } 
        else
        {
            TransitionToLevel();
            m_currentLevelIndex += 1;
            m_currentLevelType = m_Levels.OrderList[m_currentLevelIndex];
            PickFromType(m_currentLevelType);
        }
    }

    public void LoadPreviousLevel()
    {
        if (m_currentLevelIndex <= 0)
        {
            Debug.LogWarning("No previous level !");
            return;
        }
        else
        {
            TransitionToLevel();
            m_currentLevelIndex += -1;
            m_currentLevelType = m_Levels.OrderList[m_currentLevelIndex];
            PickFromType(m_currentLevelType);
        }
    }

    public void RestartLevel()
    {
        if (m_Levels.PrevLevelOrder != null && m_currentLevelIndex != -1)
        {
            m_Levels = m_Levels.PrevLevelOrder;
        }

        TransitionToLevel();

        m_currentLevelIndex = 0;
        m_currentLevelType = m_Levels.OrderList[m_currentLevelIndex];
        PickFromType(m_currentLevelType);
    }

    private void PickFromType(string currentLevelType)
    {
        switch (currentLevelType)
        {
            case "Hub":
                LoadLevel(m_Levels.LevelLists.HubList);
                break;

            case "Easy_Room":
                LoadLevel(m_Levels.LevelLists.RoomEasyList);
                break;
            case "Normal_Room":
                LoadLevel(m_Levels.LevelLists.RoomNormalList);
                break;
            case "Hard_Room":
                LoadLevel(m_Levels.LevelLists.RoomHardList);
                break;

            case "Easy_Corridor":
                LoadLevel(m_Levels.LevelLists.CorridorEasyList);
                break;
            case "Normal_Corridor":
                LoadLevel(m_Levels.LevelLists.CorridorNormalList);
                break;
            case "Hard_Corridor":
                LoadLevel(m_Levels.LevelLists.CorridorHardList);
                break;

            case "Shop":
                LoadLevel(m_Levels.LevelLists.ShopList);

                break;
            case "Easy_Final":
                LoadLevel(m_Levels.LevelLists.FinalEasyList);
                break;
            case "Normal_Final":
                LoadLevel(m_Levels.LevelLists.FinalNormalList);
                break;
            case "Hard_Final":
                LoadLevel(m_Levels.LevelLists.FinalHardList);
                break;
        }
    }
    public void PickFromTypeAndIndex(string currentLevelType, int index)
    {
        TransitionToLevel();
        m_currentLevelType = currentLevelType;

        switch (currentLevelType)
        {
            case "Hub":
                _currentLevel = Instantiate(m_Levels.LevelLists.HubList[index], Vector3.zero, Quaternion.identity);
                break;

            case "Easy_Room":
                _currentLevel = Instantiate(m_Levels.LevelLists.RoomEasyList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Normal_Room":
                _currentLevel = Instantiate(m_Levels.LevelLists.RoomNormalList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Hard_Room":
                _currentLevel = Instantiate(m_Levels.LevelLists.RoomHardList[index], Vector3.zero, Quaternion.identity);
                break;

            case "Easy_Corridor":
                _currentLevel = Instantiate(m_Levels.LevelLists.CorridorEasyList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Normal_Corridor":
                _currentLevel = Instantiate(m_Levels.LevelLists.CorridorNormalList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Hard_Corridor":
                _currentLevel = Instantiate(m_Levels.LevelLists.CorridorHardList[index], Vector3.zero, Quaternion.identity);
                break;

            case "Shop":
                _currentLevel = Instantiate(m_Levels.LevelLists.ShopList[index], Vector3.zero, Quaternion.identity);
                break;

            case "Easy_Final":
                _currentLevel = Instantiate(m_Levels.LevelLists.FinalEasyList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Normal_Final":
                _currentLevel = Instantiate(m_Levels.LevelLists.FinalNormalList[index], Vector3.zero, Quaternion.identity);
                break;
            case "Hard_Final":
                _currentLevel = Instantiate(m_Levels.LevelLists.FinalHardList[index], Vector3.zero, Quaternion.identity);
                break;
        }

        ExcractSpawnPoint(_currentLevel);

        OnRoomStart?.Invoke();

        m_loadSurface = true;
    }

    private void TransitionToLevel()
    {
        m_enemyManagerScript.DestroyAll();

        m_transition.LoadTransition();

        m_spawnPoint = null;

        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
    }

    private void LoadLevel(GameObject[] levels)
    {

        int rand = UnityEngine.Random.Range(0, levels.Length);
        _currentLevel = Instantiate(levels[rand], Vector3.zero, Quaternion.identity);

        ExcractSpawnPoint(_currentLevel);

        OnRoomStart?.Invoke();

        m_loadSurface = true;
    }

    private void ExcractSpawnPoint(GameObject levels) 
    {
        m_spawnPoint = levels.GetComponent<RoomInfo>().SpawnPoint.transform;
        PlayerController.Instance.transform.position = m_spawnPoint.position;
    }

    //private void SpawnPlayer() 
    //{
    //    m_spawnPoint = GameObject.FindWithTag("PlayerSpawn")?.transform;

    //    Debug.Log(m_spawnPoint.gameObject.name);

    //    if (m_spawnPoint == null)
    //    {
    //        Debug.LogError("No player spawn found in level. Create a Transform GO with the \"PlayerSpawn\" tag.");
    //        return;
    //    }

    //    PlayerController.Instance.transform.position = m_spawnPoint.position;
    //}

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
            case "Hub":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[0]);
                break;
            case "Easy_Room":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[1]);
                break;
            case "Normal_Room":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[2]);
                break;
            case "Hard_Room":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[3]);
                break;

            case "Easy_Corridor":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[4]);
                break;
            case "Normal_Corridor":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[5]);
                break;
            case "Hard_Corridor":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[6]);
                break;

            case "Shop":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[7]);
                break;

            case "Easy_Final":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[8]);
                break;
            case "Normal_Final":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[9]);
                break;
            case "Hard_Final":
                selected = EditorGUILayout.Popup("Specified Level", selected, room.Levels.LevelNames[10]);
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