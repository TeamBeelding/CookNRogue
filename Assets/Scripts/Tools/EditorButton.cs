
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        Enemy enemyScript = (Enemy)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Attack"))
        {
            enemyScript.Attack();
        }
        
        if (GUILayout.Button("Take hit"))
        {
            enemyScript.TakeDamage();
        }

        if (GUILayout.Button("Recoil"))
        {
            enemyScript.KnockBack();
        }

        if (GUILayout.Button("Color Feedback"))
        {
            enemyScript.ColorFeedback();
        }

        if (GUILayout.Button("Kill Enemy"))
        {
            enemyScript.KillEnemy();
        }
    }
}
#endif