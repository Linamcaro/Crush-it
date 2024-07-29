using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cameraSizeOffset;// move camera horizontal
    [SerializeField] private float cameraVerticalOffset; // move camera vertical

    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private PiecesSO availablePieces;

    [SerializeField] private Tile[,] activeTiles;
    [SerializeField] private Piece[,] activePieces;

    [SerializeField] private int pieceZpos;
    [SerializeField] private Tile startTilePos;
    [SerializeField] private Tile endTilePos;



    // Start is called before the first frame update
    void Start()
    {
        BoardSetUp();
        PositionCamera();
        SetupPieces();
    }

  
    //adjust camera position to the center
    private void BoardSetUp()
    {

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x, y, -3);

                // Instantiate each grid cell
                var gridObject = Instantiate(tilePrefab, position, Quaternion.identity);
                gridObject.transform.SetParent(transform);
                gridObject.name = "[" + x + "," + y + "]";

                //save the tile reference to the two dimension array
                activeTiles[x, y] = gridObject.GetComponent<Tile>();

                activeTiles[x, y]?.Setup(x,y,this);

                

            }
        }
    }

    /// <summary>
    /// Position camera to the center of the grid
    /// </summary>
    private void PositionCamera()
    {
        float newPosX = (float) gridWidth / 2;
        float newPosy = (float) gridHeight / 2;

        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosy - 0.5f + cameraVerticalOffset, -10f);


        float horizontal = gridWidth + 1;
        float vertical = ( gridHeight / 2) + 1;

        Camera.main.orthographicSize = Math.Max(horizontal, vertical) + cameraSizeOffset;

    }

    //Creating the pieces
    private void SetupPieces()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //selecting a random piece
                var index = Random.Range(0,availablePieces.pieces.Length);

                var pieceSelected = availablePieces.pieces[index];

                // Instantiate each piece
                Vector3 position = new Vector3(x, y, pieceZpos);
                var gridObject = Instantiate(pieceSelected, position, Quaternion.identity);
                gridObject.transform.SetParent(transform);

                //Get reference to the piece
                activePieces[x, y] = gridObject.GetComponent<Piece>();
                activePieces[x, y]?.PieceSetUp(x, y, this);

            }
        }
    }

    public void OnTileSelected(Tile tileSelected)
    {
        startTilePos = tileSelected;
    }

    public void OnTileMoved(Tile tileOver)
    {
        endTilePos = tileOver;
    }

    public void OnTileDropped(Tile tileDropped)
    {
        if(startTilePos != null && endTilePos != null)
        {
            SwapTiles();
        }

        startTilePos = null;
        endTilePos = null;

    }

    /// <summary>
    /// Swap the tile to the new position
    /// </summary>
    private void SwapTiles()
    {
        //Save refence of the pieces in the two positions
        var StartPiece = activePieces[startTilePos.x, startTilePos.y];
        var EndPiece = activePieces[endTilePos.x, endTilePos.y];

        //Move the pieces to the new positions
        StartPiece.MovePiece(endTilePos.x, endTilePos.y,pieceZpos);
        EndPiece.MovePiece(startTilePos.x, startTilePos.y, pieceZpos);

        //update the coordinates in the activePieces array
        activePieces[startTilePos.x,startTilePos.y] = EndPiece;
        activePieces[endTilePos.x,endTilePos.y] = StartPiece;

    }
}




