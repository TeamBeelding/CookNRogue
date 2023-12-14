using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        private static EnemyManager _instance;

        [SerializeField]
        private List<EnemyController> _enemiesInLevel = new List<EnemyController>();
    
        //[SerializeField] private int numOfEnemies = 0;
        [SerializeField] private int enemiesCount = 0;

        public event Action OnAllEnnemiesKilled;

        public static EnemyManager Instance
        {
            get { return _instance; }
        }

        public EnemyController[] EnemiesInLevel
        {
            get { return _enemiesInLevel.ToArray(); }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }

            _instance = this;
            DontDestroyOnLoad(this);
        }

        public void AddEnemyToLevel(EnemyController enemy)
        {
            _enemiesInLevel.Add(enemy);
            enemiesCount++;
        }

        public void RemoveEnemyFromLevel(EnemyController enemy)
        {
            //HACK : List.Remove does not remove elements inside list for EnemyController type
            //So we check directly the instance ids to remove
            //(There is probably a better way to do this)
            for (int i = _enemiesInLevel.Count; i-- > 0;)
            {
                if (_enemiesInLevel[i].GetInstanceID() == enemy.GetInstanceID())
                {
                    _enemiesInLevel.RemoveAt(i);
                    enemiesCount--;
                    break;
                }
            }

            //if (numOfEnemies <= 0 && OnAllEnnemiesKilled != null)
            //{
            //    OnAllEnnemiesKilled?.Invoke();
            //}
        }

        public void LastAIDying()
        {
            Debug.Log("ça marche sa merde");
            OnAllEnnemiesKilled?.Invoke();
        }

        public int GetNumOfEnemies() => enemiesCount;

        public void DestroyAll()
        {
            if (_enemiesInLevel != null)
            {
                int StartCount = _enemiesInLevel.Count;
                for (int i = StartCount - 1; i >= 0; i--)
                {
                    var current = _enemiesInLevel[i];
                    Destroy(current.gameObject);
                }
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyManager))]
    public class EnemyManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            //EnemyManager enemies = (EnemyManager)target;

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("TOOLS: ", "");
            GuiLine(1);

            if (GUILayout.Button("Kill All Enemies In Level"))
            {
                EnemyManager.Instance.DestroyAll();
            }

            EditorGUILayout.Separator();
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
}