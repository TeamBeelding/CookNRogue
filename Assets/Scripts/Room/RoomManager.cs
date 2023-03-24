using System;
using UnityEngine;
using Unity.AI.Navigation;


public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [SerializeField]
    private GameObject[] m_easyLevels;
    [SerializeField]
    private GameObject[] m_hardLevels;

    //[SerializeField]
    //private LevelOrderData m_hardLevels;

    private string[] _levelNames;

    public string[] LevelNames
    {
        get => _levelNames;
    }

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
        InitLevelNames();

        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance pr�c�dente (s�curit�...s�curit�...)

        instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        LoadRandomLevel();

        m_navMeshSurface.BuildNavMesh();
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

    public void LoadRandomLevel()
    {

        TransitionToLevel();

        if (!isHard)
        {
            LoadLevel(m_easyLevels);
        }
        else
        {
            LoadLevel(m_hardLevels);
        }
    }

    public void LoadPecifiedLevel(int i)
    {

        TransitionToLevel();

        if (i < 3)
        {
            isHard = false;
            _currentLevel = Instantiate(m_easyLevels[i], Vector3.zero, Quaternion.identity);
            m_loadSurface = true;
        }
        else
        {
            isHard = true;
            _currentLevel = Instantiate(m_hardLevels[i - m_easyLevels.Length], Vector3.zero, Quaternion.identity);
            m_loadSurface = true;
        }
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

    private void OnEnable()
    {
        InitLevelNames();
    }

    public void InitLevelNames()
    {
        _levelNames = new string[m_easyLevels.Length + m_hardLevels.Length];

        for (int i = 0; i < m_easyLevels.Length; i++)
        {
            _levelNames[i] = m_easyLevels[i].name;
        }

        for (int i = m_easyLevels.Length; i < m_easyLevels.Length + m_hardLevels.Length; i++)
        {
            _levelNames[i] = m_hardLevels[i - m_easyLevels.Length].name;
        }
    }

}