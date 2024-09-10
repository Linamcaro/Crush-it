using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodCollection", menuName = "Food/FoodCollection")]
public class FoodCollectionSO : ScriptableObject
{
    public List<FoodItemSO> foodItems;
}
