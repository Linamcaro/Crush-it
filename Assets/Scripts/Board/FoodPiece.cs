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

    [SerializeField] private BoardManager boardManager;

    // Method to set the food type and sprite for the piece
    public void SetFood(string foodName, Sprite foodSprite)
    {
        this.foodName = foodName;
        spriteRenderer.sprite = foodSprite; // Assign the sprite to the piece
    }


    // Method to set the  food coordinates for the piece
    public void SetCoordinates(int xCoord, int yCoord, BoardManager board)
    {
        x = xCoord;
        y = yCoord;
        boardManager = board;
    }


    void OnMouseDown()
    {

        boardManager.OnPieceSelected(this);
        
    }
    //hover mouse
    public void OnMouseEnter()
    {
        boardManager.OnPieceMoved(this);
    }

    private void OnMouseUp()
    {
        boardManager.OnPieceDropped(this);
    }


    /// <summary>
    /// Move position of the piece
    /// </summary>
    /// <param name="destX"></param>
    /// <param name="destY"></param>
    /// <param name="offset"></param>
    public void MovePiece(int destX, int destY, Vector2 offset)
    {

        Vector2 targetPosition = new Vector2(destX, destY) ; // Calculate the world position


        transform.DOMove(targetPosition, pieceMovementTime).SetEase(Ease.OutBack).OnComplete (() =>
        {
            x = destX; // Update grid position after the movement
            y = destY;
        });
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
