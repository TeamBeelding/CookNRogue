using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyJumpOnMove : MonoBehaviour
{
    [SerializeField] private AnimationCurve m_jumpCurve;
    [SerializeField] private float m_jumpFrequency = 0.05f;
    private float jumpTimer = 0f;
    private float currentJumpHeight = 0f;
    [SerializeField] private float m_jumpHeight = 0f;
    private float jumpRatio;

    private EnemyController enemy;
    private bool isMoving = false;
    
    [SerializeField] private bool GuiDebug;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    private void Update()
    {
            UpdateJump();
    }

    private void UpdateJump()
    {
        Vector3 localPos = transform.localPosition;
        
        localPos.y -= currentJumpHeight;
        
        if (enemy.IsMoving())
        {
            jumpTimer += Time.deltaTime;
        
            jumpRatio = (jumpTimer % m_jumpFrequency) / m_jumpFrequency;
        
            jumpRatio = m_jumpCurve.Evaluate(jumpRatio);
        
            currentJumpHeight = jumpRatio * m_jumpHeight;
        
            localPos.y += currentJumpHeight;
        }
        else
        {
            jumpTimer = 0;
            currentJumpHeight = 0;
        }
        
        transform.localPosition = localPos;
    }
    
    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }
    
    private void OnGUI()
    {
        if (!GuiDebug) return;
        
        GUILayout.Label($"ratio : {jumpRatio}, {jumpTimer}");
        GUILayout.Label($"current jump height : {currentJumpHeight}");
    }
}
