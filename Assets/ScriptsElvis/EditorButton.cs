using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        Enemy enemyScript = (Enemy)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Take hit"))
        {
            enemyScript.TakeDamage();
        }

        if (GUILayout.Button("Recoil"))
        {
            
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
