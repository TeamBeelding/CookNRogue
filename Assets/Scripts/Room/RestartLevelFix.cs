using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enemy;

public class RestartLevelFix : MonoBehaviour
{
    [SerializeField]
    private TransitionController m_transition;

    // Start is called before the first frame update
    public static RestartLevelFix Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(int index) 
    {
        /*m_transition.LoadTransition();

        StartCoroutine(ILoadNextScene());

        IEnumerator ILoadNextScene()
        {
            DestroyInstances();
            SceneManager.LoadScene(index);
        }*/

        DestroyInstances();
        SceneManager.LoadScene(index);
    }

    void DestroyInstances()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.DestroyAI();
            Destroy(PoolManager.Instance.gameObject);
        }
        if (PlayerController.Instance != null)
        {
            Destroy(PlayerController.Instance.gameObject);
        }
        if(EnemyManager.Instance != null)
        {
            Destroy(EnemyManager.Instance.gameObject);
        }
        if (MissileManager.instance != null)
        {
            Destroy(MissileManager.instance.gameObject);
        }
    }
}