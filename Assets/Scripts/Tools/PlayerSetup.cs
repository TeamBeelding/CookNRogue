#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class PlayerSetup : EditorWindow
{
    private PlayerController _playerController;
    private PlayerAttack _playerAttack;
    
    [MenuItem("Toolbox/Player Settings")]
    static void InitWindow()
    {
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        PlayerAttack[] playerAttacks = FindObjectsOfType<PlayerAttack>();

        if (playerControllers.Length > 1 || playerAttacks.Length > 1)
        {
            Debug.LogError("Mutliple player scripts found in scene. Only one allowed.");
            return;
        }
        if (playerControllers.Length == 0 || playerAttacks.Length == 0)
        {
            Debug.LogError("Player scripts missing.");
            return;
        }
        
        PlayerSetup playerSetupWindow = GetWindow<PlayerSetup>();
        playerSetupWindow._playerController = playerControllers[0];
        playerSetupWindow._playerAttack = playerAttacks[0];
        playerSetupWindow.titleContent = new GUIContent("Player Settings");
        playerSetupWindow.Show();
    }
    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        GUILayout.Label("Character Properties", EditorStyles.boldLabel);
        _playerController.m_rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", _playerController.m_rotationSpeed);
        _playerController.m_moveSpeed = EditorGUILayout.FloatField("Move Speed", _playerController.m_moveSpeed);
        _playerController.m_maxMoveSpeed = EditorGUILayout.FloatField("Max Move Speed", _playerController.m_maxMoveSpeed);
        _playerController.m_moveDrag = EditorGUILayout.FloatField("Move Drag", _playerController.m_moveDrag);
        _playerController.m_interactionRange = EditorGUILayout.FloatField("Interaction Range", _playerController.m_interactionRange);
        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        GUILayout.Label("Physics & Movements", EditorStyles.boldLabel);
        _playerAttack.size = EditorGUILayout.FloatField("Size", _playerAttack.size);
        _playerAttack.speed = EditorGUILayout.FloatField("Speed", _playerAttack.speed);
        _playerAttack.drag = EditorGUILayout.FloatField("Drag", _playerAttack.drag);
        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        GUILayout.Label("Attack", EditorStyles.boldLabel);
        _playerAttack.heavyAttackDelay = EditorGUILayout.FloatField("Heavy Attack Delay", _playerAttack.heavyAttackDelay);
        _playerAttack.heavyDamage = EditorGUILayout.FloatField("Heavy Attack Damage", _playerAttack.heavyDamage);
        _playerAttack.lightAttackDelay = EditorGUILayout.FloatField("Light Attack Delay", _playerAttack.lightAttackDelay);
        _playerAttack.lightDamage = EditorGUILayout.FloatField("Light Attack Damage", _playerAttack.lightDamage);
        GUILayout.EndVertical();
    }
}

#endif