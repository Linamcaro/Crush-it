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

    [SerializeField] private float scaleSpeed;
    [SerializeField] private float removeScaleUpSpeed;
    [SerializeField] private float removeScaleDownSpeed;
    [SerializeField] private float removePieceScale;

    public Type pieceType;

    public void PieceSetUp(int xCoord, int yCoord, GameBoard board)
    {
        x = xCoord;
        y = yCoord;
        gameBoard = board;

        transform.localScale = Vector3.one * scaleSpeed;
        transform.DOScale(Vector3.one, scaleSpeed);
    }

    public void MovePiece(int destX, int destY, float destZ = - 5f)
    {
        //Debug.Log($"Moving piece from ({x}, {y}) to ({destX}, {destY})");

        transform.DOMove(new Vector3(destX, destY, destZ), pieceMovementTime).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = destX;
            y = destY;


        };

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

        }else 
        {
            Destroy(gameObject); 
        }
    }

    /*
    [ContextMenu("Test Move")]
    public void MoveTest()
    {
        MovePiece(0, 0, -5);
    }*/
}

