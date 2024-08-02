using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{
    //pieces tipe
    public enum Type
    {
        elephant,
        giraffe,
        hippo,
        monkey,
        panda,
        parrot,
        penguin,
        pig,
        rabbit,
        snake
    };

    [SerializeField] public int x;
    [SerializeField] public int y;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private float pieceMovementTime;

    public Type pieceType;

    public void PieceSetUp(int xCoord, int yCoord, GameBoard board)
    {
        x = xCoord;
        y = yCoord;
        gameBoard = board;
    }

    public void MovePiece(int destX, int destY, float destZ = - 5f)
    {
        //Debug.Log($"Moving piece from ({x}, {y}) to ({destX}, {destY})");

        transform.DOMove(new Vector3(destX, destY, destZ), pieceMovementTime).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = destX;
            y = destY;

            //Debug.Log($"Piece moved to ({x}, {y})");
        };

        //Debug.Log($"Piece moved to ({x}, {y})");
    }

    /*
    [ContextMenu("Test Move")]
    public void MoveTest()
    {
        MovePiece(0, 0, -5);
    }*/
}

