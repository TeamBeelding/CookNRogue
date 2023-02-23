using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyJumpOnMove : MonoBehaviour
{
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpFrequency = 0.05f;
    private float jumpTimer = 0f;
    private float currentJumpHeight = 0f;
    [SerializeField] private float jumpHeight = 0f;
    private float jumpRatio;

    private EnemyController enemy;

    [SerializeField] private bool GuiDebug;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
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

            jumpRatio = (jumpTimer % jumpFrequency) / jumpFrequency;

            jumpRatio = jumpCurve.Evaluate(jumpRatio);

            currentJumpHeight = jumpRatio * jumpHeight;

            localPos.y += currentJumpHeight;
        }
        else
        {
            jumpTimer = 0;
            currentJumpHeight = 0;
        }
        
        transform.localPosition = localPos;
    }
    
    private void OnGUI()
    {
        if (!GuiDebug) return;
        
        GUILayout.Label($"ratio : {jumpRatio}, {jumpTimer}");
        GUILayout.Label($"current jump height : {currentJumpHeight}");
    }
}
