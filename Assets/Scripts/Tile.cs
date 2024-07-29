using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{ 
    [SerializeField] public int x;
    [SerializeField] public int y;
    [SerializeField] private GameBoard gameBoard;

    public void Setup(int horizontal, int vertical, GameBoard board)
    {
        x = horizontal;
        y = vertical;
        gameBoard = board;
    }

    public void OnMouseDown()
    {
        gameBoard.OnTileSelected(this);
    }

    public void OnMouseEnter()
    {
        gameBoard.OnTileMoved(this);
    }

    private void OnMouseUp()
    {
        gameBoard.OnTileDropped(this);
    }


}
