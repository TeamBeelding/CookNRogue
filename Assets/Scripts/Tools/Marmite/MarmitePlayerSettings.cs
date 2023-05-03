#if UNITY_EDITOR

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class MarmitePlayerSettings
{
    private PlayerController m_playerController;
    private PlayerAttack m_playerAttack;
    
    
    #region PlayerController
    [BoxGroup("PlayerController", true, true)]
    [InfoBox("No PlayerController script found in scene.", InfoMessageType.Error, "IsPlayerControllerNull")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetMaxHealth")]
    public float maxHealth;
    [BoxGroup("PlayerController")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetRotationSpeed")]
    public float rotationSpeed;
    [BoxGroup("PlayerController")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetMoveSpeed")]
    public float moveSpeed;
    [BoxGroup("PlayerController")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetMaxMoveSpeed")]
    public float maxMoveSpeed;
    [BoxGroup("PlayerController")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetMoveDrag")]
    public float moveDrag;
    [BoxGroup("PlayerController")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetDashDuration")]
    public float dashDuration;
    [BoxGroup("PlayerController")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetDashForce")]
    public float dashForce;
    [BoxGroup("PlayerController")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetInteractionRange")]
    public float interactionRange;
    
    #endregion

    #region PlayerAttack
    
    [BoxGroup("PlayerAttack", true, true)]
    [InfoBox("No PlayerAttack script found in scene.", InfoMessageType.Error, "IsPlayerAttackNull")]

    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetSize")]
    public float size;
    [BoxGroup("PlayerAttack")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetSpeed")]
    public float speed;
    [BoxGroup("PlayerAttack")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetDrag")]
    public float drag;
    [BoxGroup("PlayerAttack")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetHeavyDamage")]
    public float damage;
    [BoxGroup("PlayerAttack")]
    [EnableIf("m_playerController")]
    [CustomValueDrawer("SetShootCooldown")]
    public float shootCooldown;
    
    #endregion
    
    public MarmitePlayerSettings()
    {
        m_playerController = GameObject.FindObjectOfType<PlayerController>();
        if (m_playerController == null)
        {
            Debug.LogAssertion("No PlayerController script Found in scene");
            return;
        }

        maxHealth = m_playerController.m_maxHealthValue;
        rotationSpeed = m_playerController.m_rotationSpeed;
        moveSpeed = m_playerController.m_moveSpeed;
        maxMoveSpeed = m_playerController.m_maxMoveSpeed;
        moveDrag = m_playerController.m_moveDrag;
        dashDuration = m_playerController.m_dashDuration;
        dashForce = m_playerController.m_dashForce;
        interactionRange = m_playerController.m_interactionRange;
        
        m_playerAttack = GameObject.FindObjectOfType<PlayerAttack>();
        if (m_playerAttack == null)
        {
            Debug.LogAssertion("No PlayerAttack script Found in scene");
            return;
        }
        
        size = m_playerAttack._size;
        speed = m_playerAttack._speed;
        drag = m_playerAttack._drag;
        damage = m_playerAttack._damage;
        shootCooldown = m_playerAttack._shootCooldown;
    }

    #region PlayerController
    
    private float SetMaxHealth(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);
        return EditorGUILayout.FloatField(label, m_playerController.m_maxHealthValue = value);
    }
    private float SetRotationSpeed(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerController.m_rotationSpeed = value);
    }
    private float SetMoveSpeed(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerController.m_moveSpeed = value);
    }
    private float SetMaxMoveSpeed(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerController.m_maxMoveSpeed = value);
    }
    private float SetMoveDrag(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerController.m_moveDrag = value);
    }
    private float SetDashDuration(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerController.m_dashDuration = value);
    }
    private float SetDashForce(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerController.m_dashForce = value);
    }
    private float SetInteractionRange(float value, GUIContent label)
    {
        if (m_playerController == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerController.m_interactionRange = value);
    }
    #endregion

    #region PlayerAttack
    
    private float SetSize(float value, GUIContent label)
    {
        if (m_playerAttack == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerAttack._size = value);
    }
    private float SetSpeed(float value, GUIContent label)
    {
        if (m_playerAttack == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerAttack._speed = value);
    }
    private float SetDrag(float value, GUIContent label)
    {
        if (m_playerAttack == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerAttack._drag = value);
    }
    private float SetHeavyDamage(float value, GUIContent label)
    {
        if (m_playerAttack == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerAttack._damage = value);
    }

    private float SetShootCooldown(float value, GUIContent label)
    {
        if (m_playerAttack == null) return EditorGUILayout.FloatField(label, 0);;
        return EditorGUILayout.FloatField(label, m_playerAttack._shootCooldown = value);
    }

    #endregion

    private bool IsPlayerControllerNull()
    {
        return m_playerController == null;
    }
    private bool IsPlayerAttackNull()
    {
        return m_playerAttack == null;
    }
    
}
#endif