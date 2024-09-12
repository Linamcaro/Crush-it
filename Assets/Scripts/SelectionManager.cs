using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private static SelectionManager instance;
    public static SelectionManager Instance
    {
        get
        {
            return instance;
        }
    }

    private FoodPiece firstPiecePosition;
    private FoodPiece secondPiecePosition;

    [SerializeField] private BoardManager boardManager; // Reference to BoardManager for match checking



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
        else
        {
            instance = this;
        }

    }



    /// <summary>
    /// called when player selects a tile
    /// </summary>
    /// <param name="tileSelected"></param>
    public void OnPieceSelected(FoodPiece tileSelected)
    {
      
       firstPiecePosition = tileSelected;
        
    }

    /// <summary>
    /// called when player holds selection and move it to other position
    /// </summary>
    /// <param name="tileOver"></param>
    public void OnPieceMoved(FoodPiece tileOver)
    {
        
        secondPiecePosition = tileOver;
        
    }

    /// <summary>
    /// Called when player release the tile selected
    /// </summary>
    /// <param name="tileDropped"></param>
    public void OnPieceDropped(FoodPiece tileDropped)
    {
        
        if (firstPiecePosition != null && secondPiecePosition != null && IsCloseTo(firstPiecePosition, secondPiecePosition))
        {
            boardManager.SwapTiles(firstPiecePosition, secondPiecePosition);
        }
    }

    /// <summary>
    /// check if the target tiles are next to each other
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool IsCloseTo(FoodPiece start, FoodPiece end)
    {
        //check if the destination of the piece to move in x is next to the current position 
        if (Math.Abs((start.x - end.x)) == 1 && start.y == end.y)
        {
            return true;
        }
        //check if the destination of the piece to move in Y is next to the current position 
        else if (Math.Abs((start.y - end.y)) == 1 && start.x == end.x)
        {
            return true;
        }

        return false;
    }


}
