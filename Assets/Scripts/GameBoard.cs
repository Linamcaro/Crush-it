using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        activeTiles = new Tile[gridWidth, gridHeight];
        activePieces = new Piece[gridWidth, gridHeight];

        BoardSetUp();
        PositionCamera();
        SetupPieces();
    }


    /// <summary>
    /// called when player selects a tile
    /// </summary>
    /// <param name="tileSelected"></param>
    public void OnTileSelected(Tile tileSelected)
    {
        startTilePos = tileSelected;
    }

    /// <summary>
    /// called when player holds selection and move it to other position
    /// </summary>
    /// <param name="tileOver"></param>
    public void OnTileMoved(Tile tileOver)
    {
        endTilePos = tileOver;
    }

    /// <summary>
    /// Called when player release the tile selected
    /// </summary>
    /// <param name="tileDropped"></param>
    public void OnTileDropped(Tile tileDropped)
    {
        if (startTilePos != null && endTilePos != null && IsCloseTo(startTilePos, endTilePos))
        {
            SwapTiles();
        }

        startTilePos = null;
        endTilePos = null;

    }

        /// <summary>
        /// adjust camera position to the center
        /// </summary>
        private void BoardSetUp()
    {

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x, y, pieceZpos);

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

    /// <summary>
    /// Creating the pieces
    /// </summary>
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

    /// <summary>
    /// check if the target tiles are next to each other
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private bool IsCloseTo(Tile start, Tile end)
    {
        //check if the destination of tile to move in x is next to the current position 
        if (Math.Abs(start.x - end.x) == 1 && start.y == end.y)
        {
            return true;
        }
        //check if the destination of tile to move in Y is next to the current position 
        else if(Math.Abs(start.y - end.y) == 1 && start.x == end.x)
        { 
            return true;
        }
        else { return false; } 
    }

    /// <summary>
    /// Check if there are matches in a given direction
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="direction"></param>
    /// <param name="minPieces"></param>
    /// <returns></returns>
    private List<Piece> GetMatchByDirection(int xPos, int yPos, Vector2 direction, int minPieces = 3)
    {
        //List of found pieces that matches
        List<Piece> matches = new List<Piece>();
        //Start piece
        Piece startPiece = activePieces[xPos, yPos];
        matches.Add(startPiece);

        int nextX;
        int nextY;
        int maxVal = Math.Max(gridWidth, gridHeight);

        for(int i = 1; i < maxVal; i++)
        {
            nextX = xPos + ((int) direction.x * i);
            nextY = yPos + ((int) direction.y * i);

            if(nextX >= 0 && nextX < gridWidth && nextY >= 0 && nextY < gridHeight)
            {
                //reference to the next piece
                var nextPiece = activePieces[nextX, nextY];

                if(nextPiece != null && nextPiece.pieceType == startPiece.pieceType )
                {

                    matches.Add(nextPiece);

                }
                else
                {
                    break;
                }
            }
        }

        if (matches.Count >= minPieces)
        {
            return matches;
        }

        return null;
    }

    /// <summary>
    /// Check the total matches in the 4 directions
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="minPieces"></param>
    /// <returns></returns>
    public List<Piece> GetMatchByPiece(int xPos, int yPos, int minPieces = 3)
    {

        var matchAbove = GetMatchByDirection(xPos, yPos, new Vector2(0, 1), 2) ?? new List<Piece>();
        var matchBelow = GetMatchByDirection(xPos, yPos, new Vector2(0, -1), 2) ?? new List<Piece>();
        var matchRight = GetMatchByDirection(xPos, yPos, new Vector2(1, 0), 2) ?? new List<Piece>();
        var matchLeft = GetMatchByDirection(xPos, yPos, new Vector2(-1, 0), 2) ?? new List<Piece>();

        //joint the lists
        var verticalMatch = matchAbove.Union(matchBelow).ToList();
        var horizontalMatch = matchRight.Union(matchLeft).ToList();

        var totalMatches = new List<Piece>();

        if (verticalMatch.Count >= minPieces)
        {
            totalMatches = totalMatches.Union(verticalMatch).ToList();
        }

        if (horizontalMatch.Count >= minPieces)
        {
            totalMatches.Union(horizontalMatch).ToList();
        }

        return totalMatches;

    }

}




