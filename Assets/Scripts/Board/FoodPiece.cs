using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodPiece : MonoBehaviour
{

    [SerializeField] private float pieceMovementTime;

    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer to display the food item
    public string foodName; // Type of food (e.g., "Hamburger", "Pizza", etc.)

    public int x;
    public int y;



    // Method to set the food type and sprite for the piece
    public void SetFood(string foodName, Sprite foodSprite, int xCoord, int yCoord)
    {
        this.foodName = foodName;
        spriteRenderer.sprite = foodSprite; // Assign the sprite to the piece
        x = xCoord;
        y = yCoord;

    }


    void OnMouseDown()
    {
        
         SelectionManager.Instance.OnPieceSelected(this);
        
    }
    //hover mouse
    public void OnMouseEnter()
    {
        SelectionManager.Instance.OnPieceMoved(this);
    }

    private void OnMouseUp()
    {
        SelectionManager.Instance.OnPieceDropped(this);
    }

    public void MovePiece(int destX, int destY, Vector2 offset)
    {
        Vector2 targetPosition = new Vector2(destX, destY) - offset; // Calculate the world position

        transform.DOMove(targetPosition, pieceMovementTime).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = destX; // Update grid position after the movement
            y = destY;


        };

    }

}
