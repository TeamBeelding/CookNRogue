using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MealOfTheDay : Item, ISubItem
{
    [SerializeField] MealOfTheDayData _MOTDdata;
    bool hasTriggered = false;
    
    protected override void Update()
    {
        base.Update();
    }
    public override void Interact(string tag)
    {
        base.Interact(tag);
        ApplyItem();
    }

    public void ApplyItem()
    {
        if (!CanTrigger())
            return;

        ApplyItemRoutine();

        for(int i = 0; i < _MOTDdata.playerUpgradeHealth; i++)
        {
            PlayerHealth.instance.UpgradeMaxHealth(_MOTDdata.playerUpgradeHealth);
        }
        
    }

}
