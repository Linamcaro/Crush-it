using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FoodItem", menuName = "Food/FoodItem")]
public class FoodItemSO : ScriptableObject
{
 
        public string foodName; // E.g., "Hamburger", "Pizza", etc.
        public Sprite foodSprite; // The sprite representing this foo

}
