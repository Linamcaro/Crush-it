using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoodPiece : MonoBehaviour
{

    [SerializeField] private float pieceMovementTime;

    [SerializeField] private float removeScaleUpSpeed;
    [SerializeField] private float removeScaleDownSpeed;
    [SerializeField] private float removePieceScale;

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


    /// <summary>
    /// Move position of the piece
    /// </summary>
    /// <param name="destX"></param>
    /// <param name="destY"></param>
    /// <param name="offset"></param>
    public void MovePiece(int destX, int destY, Vector2 offset)
    {
        Debug.Log("Moving piece: " + this.name + " to position (" + destX + ", " + destY + ")");

        Vector2 targetPosition = new Vector2(destX, destY) - offset; // Calculate the world position

        Debug.Log("The target position is: " + targetPosition);

        transform.DOMove(targetPosition, pieceMovementTime).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = destX; // Update grid position after the movement
            y = destY;

            Debug.Log("Piece moved to x: " + x + "and y: " + y + "destination" );

        };

         Debug.Log("Piece moved: " + this.name + " to actual position: " + transform.position);
    }

    public void RemovePiece(bool animated)
    {
        if (animated)
        {
            transform.DORotate(new Vector3(0, 0, -120f), 0.12f);
            transform.DOScale(Vector3.one * removePieceScale, removeScaleUpSpeed).onComplete = () =>
            {
                transform.DOScale(Vector3.zero, removeScaleDownSpeed).onComplete = () =>
                {
                    Destroy(gameObject);
                };
            };

        }
        else
        {
            Destroy(gameObject);
        }
    }

}
