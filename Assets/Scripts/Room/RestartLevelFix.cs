using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void RestartLevel() 
    {
        m_transition.LoadTransition();

        StartCoroutine(ILoadNextScene());

        IEnumerator ILoadNextScene()
        {
            yield return new WaitForSeconds(0.1f);
            SceneManager.LoadScene(1);
        }
    }

}