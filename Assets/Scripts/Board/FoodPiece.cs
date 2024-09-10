using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPiece : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer to display the food item
    public string foodName; // Type of food (e.g., "Hamburger", "Pizza", etc.)

    // Method to set the food type and sprite for the piece
    public void SetFood(string foodName, Sprite foodSprite)
    {
        this.foodName = foodName;
        spriteRenderer.sprite = foodSprite; // Assign the sprite to the piece
    }

}
