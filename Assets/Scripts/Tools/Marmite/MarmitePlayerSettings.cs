#if UNITY_EDITOR

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MarmitePlayerSettings
{
    #region PlayerController
    [BoxGroup("PlayerController", true, true)]
    [CustomValueDrawer("SetMaxHealth")]
    public float MaxHealth;
    [BoxGroup("PlayerController")]
    [CustomValueDrawer("SetRotationSpeed")]
    public float RotationSpeed;
    [BoxGroup("PlayerController")]
    [CustomValueDrawer("SetMoveSpeed")]
    public float MoveSpeed;
    [BoxGroup("PlayerController")]
    [CustomValueDrawer("SetMaxMoveSpeed")]
    public float MaxMoveSpeed;
    [BoxGroup("PlayerController")]
    [CustomValueDrawer("SetMoveDrag")]
    public float MoveDrag;
    [BoxGroup("PlayerController")]
    [CustomValueDrawer("SetDashDuration")]
    public float DashDuration;
    [BoxGroup("PlayerController")]
    [CustomValueDrawer("SetDashForce")]
    public float DashForce;
    [BoxGroup("PlayerController")]
    [CustomValueDrawer("SetInteractionRange")]
    public float InteractionRange;

    #endregion

    #region PlayerAttack

    [BoxGroup("PlayerAttack", true, true)]
    [CustomValueDrawer("SetSize")]
    public float Size;
    [BoxGroup("PlayerAttack")]
    [CustomValueDrawer("SetSpeed")]
    public float Speed;
    [BoxGroup("PlayerAttack")]
    [CustomValueDrawer("SetDrag")]
    public float Drag;
    [BoxGroup("PlayerAttack")]
    [CustomValueDrawer("SetHeavyDamage")]
    public float Damage;
    [BoxGroup("PlayerAttack")]
    [CustomValueDrawer("SetShootCooldown")]
    public float ShootCooldown;
    [BoxGroup("PlayerAttack")]
    [CustomValueDrawer("SetShootColor")]
    public Color DefaultColor;

    #endregion

    #region PlayerCooking

    [BoxGroup("PlayerCooking", true, true)]
    [CustomValueDrawer("SetShowAnimDuration")]
    public float ShowAnimDuration;
    [BoxGroup("PlayerCooking")]
    [CustomValueDrawer("SetOneIngredientCookTime")]
    public float OneIngredientCookTime;
    [BoxGroup("PlayerCooking")]
    [CustomValueDrawer("SetTwoIngredientCookTime")]
    public float TwoIngredientCookTime;
    [BoxGroup("PlayerCooking")]
    [CustomValueDrawer("SetThreeIngredientCookTime")]
    public float ThreeIngredientCookTime;
    [BoxGroup("PlayerCooking")]
    [Tooltip("Random delay to spawn the QTE based on the total cook time,\r\n" +
             "Where 0 is the start of cooking and 1 is the end,\r\n" +
             "And X is the lowest possible generated value and Y the highest.")]
    [CustomValueDrawer("SetQteSpawnDelay")]
    public Vector2 QteSpawnDelay;
    [BoxGroup("PlayerCooking")]
    [Tooltip("random ammount of time in seconds available to validate the QTE," +
             " where x is the lowest possible value and y is the highest")]
    [CustomValueDrawer("SetQteDuration")]
    public Vector2 QteDuration;

    #endregion

    public MarmitePlayerSettings()
    {
        PlayerRuntimeData.GetInstance().LoadData();

        MaxHealth = PlayerRuntimeData.GetInstance().data.BaseData.MaxHealth;
        RotationSpeed = PlayerRuntimeData.GetInstance().data.BaseData.RotationSpeed;
        MoveSpeed = PlayerRuntimeData.GetInstance().data.BaseData.MoveSpeed;
        MaxMoveSpeed = PlayerRuntimeData.GetInstance().data.BaseData.MaxMoveSpeed;
        MoveDrag = PlayerRuntimeData.GetInstance().data.BaseData.MoveDrag;
        DashDuration = PlayerRuntimeData.GetInstance().data.BaseData.DashDuration;
        DashForce = PlayerRuntimeData.GetInstance().data.BaseData.DashForce;
        InteractionRange = PlayerRuntimeData.GetInstance().data.BaseData.InteractionRange;


        Size = PlayerRuntimeData.GetInstance().data.AttackData.AttackSize;
        Speed = PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed;
        Drag = PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag;
        Damage = PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage;
        ShootCooldown = PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown;
        DefaultColor = PlayerRuntimeData.GetInstance().data.AttackData.DefaultColor;


        ShowAnimDuration = PlayerRuntimeData.GetInstance().data.CookData.ShowAnimDuration;
        OneIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultOneIngredientCookTime;
        TwoIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultTwoIngredientCookTime;
        ThreeIngredientCookTime = PlayerRuntimeData.GetInstance().data.CookData.DefaultThreeIngredientCookTime;
        QteSpawnDelay = PlayerRuntimeData.GetInstance().data.CookData.QteSpawnDelay;
        QteDuration = PlayerRuntimeData.GetInstance().data.CookData.QteDuration;
    }

    #region PlayerController

    private int SetMaxHealth(int value, GUIContent label)
    {
        PlayerRuntimeData.GetInstance().SaveData();

        return EditorGUILayout.IntField(label, PlayerRuntimeData.GetInstance().data.BaseData.DefaultMaxHealth = value);
    }
    private float SetRotationSpeed(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.BaseData.RotationSpeed = value);
    }
    private float SetMoveSpeed(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.BaseData.MoveSpeed = value);
    }
    private float SetMaxMoveSpeed(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.BaseData.MaxMoveSpeed = value);
    }
    private float SetMoveDrag(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.BaseData.MoveDrag = value);
    }
    private float SetDashDuration(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.BaseData.DashDuration = value);
    }
    private float SetDashForce(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.BaseData.DashForce = value);
    }
    private float SetInteractionRange(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.BaseData.InteractionRange = value);
    }
    #endregion

    #region PlayerAttack

    private float SetSize(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.AttackData.AttackSize = value);
    }
    private float SetSpeed(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.AttackData.AttackSpeed = value);
    }
    private float SetDrag(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.AttackData.AttackDrag = value);
    }
    private float SetHeavyDamage(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.AttackData.AttackDamage = value);
    }

    private float SetShootCooldown(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.AttackData.AttackCooldown = value);
    }

    private Color SetShootColor(Color value, GUIContent label)
    {
        return EditorGUILayout.ColorField(label, PlayerRuntimeData.GetInstance().data.AttackData.DefaultColor = value, false, true, true);
    }

    #endregion

    #region PlayerCooking

    private float SetShowAnimDuration(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.CookData.ShowAnimDuration = value);
    }
    private float SetOneIngredientCookTime(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.CookData.DefaultOneIngredientCookTime = value);
    }
    private float SetTwoIngredientCookTime(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.CookData.DefaultTwoIngredientCookTime = value);
    }
    private float SetThreeIngredientCookTime(float value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, PlayerRuntimeData.GetInstance().data.CookData.DefaultThreeIngredientCookTime = value);
    }
    private Vector2 SetQteSpawnDelay(Vector2 value, GUIContent label)
    {
        return EditorGUILayout.Vector2Field(label, PlayerRuntimeData.GetInstance().data.CookData.DefaultQteSpawnDelay = value);
    }
    private Vector2 SetQteDuration(Vector2 value, GUIContent label)
    {
        return EditorGUILayout.Vector2Field(label, PlayerRuntimeData.GetInstance().data.CookData.DefaultQteDuration = value);
    }

    #endregion

}
#endif