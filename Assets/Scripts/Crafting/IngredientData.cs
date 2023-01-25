using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INGREDIENT
{
    VIANDE,
    POISSON,
    LEGUME
}
[CreateAssetMenu(fileName = "Ingredient", menuName = "Player/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public INGREDIENT ingredient;
    public Sprite sprite;
}
