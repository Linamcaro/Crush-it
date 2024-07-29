using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{

    [SerializeField] public int x;
    [SerializeField] public int y;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private float pieceMovementTime;

    //pieces tipe
    [SerializeField] private enum type
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

    [SerializeField] private type pieceType;

    public void PieceSetUp(int xCoord, int yCoord, GameBoard board)
    {
        x = xCoord;
        y = yCoord;
        gameBoard = board;
    }

    public void MovePiece(int destX, int destY, int destZ)
    {

        transform.DOMove(new Vector3(destX, destY, destZ), pieceMovementTime).SetEase(Ease.InOutCubic).onComplete =  () =>
        {
            x = destX;
            y = destY;
        };
    }

    [ContextMenu("Test Move")]
    public void MoveTest()
    {
        MovePiece(0, 0, -5);
    }

}
