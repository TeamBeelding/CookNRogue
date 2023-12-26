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
        PoolManager.Instance.DestroyAI();
        Destroy(PlayerController.Instance.gameObject);
        Destroy(EnemyManager.Instance.gameObject);
        Destroy(PoolManager.Instance.gameObject);
        Destroy(MissileManager.instance.gameObject);
    }
}