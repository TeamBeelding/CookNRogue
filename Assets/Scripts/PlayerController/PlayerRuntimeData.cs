using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuntimeData
{
    private static PlayerRuntimeData _instance;

    public static PlayerRuntimeData GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PlayerRuntimeData();
        }

        return _instance;
    }

    private PlayerRuntimeData()
    {
        LoadData();
    }

    [Serializable]
    public class PlayerBaseData
    {
        public int DefaultMaxHealth = 6;

        [NonSerialized]
        public int MaxHealth = 6;
        [NonSerialized]
        public int CurrentHealth = 6;

        [NonSerialized]
        public float InvicibilityDuration = .4f;

        public float RotationSpeed = 40f;
        public float MoveSpeed = 70f;
        public float MaxMoveSpeed = 7f;
        public float MoveDrag = 5f;

        public float DashDuration = .35f;
        public float DashForce = 1.5f;
        public float DashCooldown = 1f;

        public float InteractionRange = .5f;
    }

    [Serializable]
    public class PlayerAttackData
    {
        [NonSerialized]
        public float Ammunition = 0;
        [NonSerialized]
        public int ProjectileNumber = 0;

        [NonSerialized]
        public List<IIngredientEffects> AttackEffects = new ();

        public float AttackSize = 1;
        public float AttackSpeed = 3.5f;
        public float AttackDrag = 0;
        [NonSerialized]
        public float AttackDefaultCooldown = .1f;
        public float AttackCooldown = .1f;
        public float AttackDamage = 0;

        [ColorUsage(true, true)]
        public Color DefaultColor;

        [NonSerialized]
        [ColorUsage(true, true)]
        public Color? AttackColor;

        [NonSerialized]
        public float TimeBtwShotRafale = 0;
    }

    [Serializable]
    public class PlayerCookData
    {
        public float ShowAnimDuration = .5f;
        public float DefaultOneIngredientCookTime = 4f;
        [NonSerialized]
        public float OneIngredientCookTime = 4f;

        public float DefaultTwoIngredientCookTime = 6f;
        [NonSerialized]
        public float TwoIngredientCookTime = 6f;

        public float DefaultThreeIngredientCookTime = 8f;
        [NonSerialized]
        public float ThreeIngredientCookTime = 8f;

        public Vector2 DefaultQteSpawnDelay = new (.4f, .7f);
        [NonSerialized]
        public Vector2 QteSpawnDelay = new (.4f, .7f);

        public Vector2 DefaultQteDuration = new (.3f, .5f);
        [NonSerialized]
        public Vector2 QteDuration = new (.3f, .5f);

        [NonSerialized]
        public List<ProjectileData> Recipe = new();
    }

    [Serializable]
    public class InventoryData
    {
        public int Currency = 0;
        public bool MidasCauldron = false;
        public bool ButteredShoes = false;
        public bool MealOfTheDay = false;
        public bool LargeCauldron = false;
        public bool BigSpatule = false;
        public bool WoodenSpoon = false;
        public bool Caramel = false;
    }

    public class Data
    {
        public PlayerBaseData BaseData = new();
        public PlayerAttackData AttackData = new();
        public PlayerCookData CookData = new();
        public InventoryData InventoryData = new();
    }

    public Data data = new ();


    private string _dataFilePath = Application.dataPath + "/Resources/PlayerData.json";

    public void SaveData()
    {
        var json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(_dataFilePath, json);
    }

    private void LoadData()
    {
        data = JsonUtility.FromJson<Data>(System.IO.File.ReadAllText(_dataFilePath));
    }
}
