using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{

    [SerializeField] private int gridWidth; //gameboard width
    [SerializeField] private int gridHeight; //gameboard height
    [SerializeField] private int pieceZpos;
    [SerializeField] private float cameraSizeOffset;// move camera horizontal
    [SerializeField] private float cameraVerticalOffset; // move camera vertical

    [SerializeField] private GameObject tilePrefab; //grid tile

    [SerializeField] private PiecesSO availablePieces; //Pieces to instantiate

    private Tile[,] activeTiles; //active tiles on the board
    private Piece[,] activePieces; //active pieces on the board

    private Tile startTilePos; //start position of the tile to swap
    private Tile endTilePos; //end position of the tile to swap
    private bool isSwapingPieces = false; //True if two pieces are trying to swap 


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
    /// Creating the pieces
    /// </summary>
    private void SetupPieces()
    {
        int maxIterations = 50;
        int currentIteration = 0;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                
                var newPiece = CreatePiece(x,y);
                currentIteration = 0;

                while (HasPreviewsMatches(x,y))
                {
                    ClearPiece(x,y);
                    newPiece = CreatePiece(x,y);
                    currentIteration++;
                    if(currentIteration > maxIterations)
                    {
                        break;
                    }
                }

            }
        }
    }

    /// <summary>
    /// Destroy the game object and clean the reference
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void ClearPiece(int x, int y)
    {
        var pieceToClear = activePieces[x, y];    
        Destroy(pieceToClear.gameObject);
        activePieces[x, y] = null;


    }

    /// <summary>
    /// Instantiate a new piece at a random position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Piece CreatePiece(int x, int y)
    {   
        //selecting a random piece
        var index = Random.Range(0, availablePieces.pieces.Length);

        var pieceSelected = availablePieces.pieces[index];

        // Instantiate each piece
        Vector3 position = new Vector3(x, y, pieceZpos);
        var gridObject = Instantiate(pieceSelected, position, Quaternion.identity);
        gridObject.transform.SetParent(transform);

        //Get reference to the piece
        activePieces[x, y] = gridObject.GetComponent<Piece>();
        activePieces[x, y]?.PieceSetUp(x, y, this);

        return activePieces[x, y];
    }

    /// <summary>
    /// Position camera to the center of the grid
    /// </summary>
    private void PositionCamera()
    {
        float newPosX = (float)gridWidth / 2;
        float newPosy = (float)gridHeight / 2;

        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosy - 0.5f + cameraVerticalOffset, -10f);


        float horizontal = gridWidth + 1;
        float vertical = (gridHeight / 2) + 1;

        Camera.main.orthographicSize = Math.Max(horizontal, vertical) + cameraSizeOffset;

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
            StartCoroutine(SwapTiles());
        }

    }

    /// <summary>
    /// Swap the tile to the new position
    /// </summary>
    IEnumerator SwapTiles()
    {
        //Save refence of the pieces in the two positions
        var StartPiece = activePieces[startTilePos.x, startTilePos.y];
        var EndPiece = activePieces[endTilePos.x, endTilePos.y];

        /*Debug.Log($"StartPiece initial position: ({StartPiece.x}, {StartPiece.y})");
        Debug.Log($"EndPiece initial position: ({EndPiece.x}, {EndPiece.y})");*/

        //Move the pieces to the new positions
        StartPiece.MovePiece(endTilePos.x, endTilePos.y);
        EndPiece.MovePiece(startTilePos.x, startTilePos.y);

        //update the coordinates in the activePieces array
        activePieces[startTilePos.x,startTilePos.y] = EndPiece;
        activePieces[endTilePos.x,endTilePos.y] = StartPiece;

        yield return new WaitForSeconds(0.6f);

        //check matches for the first piece
        var startMatches = GetMatchByPiece(startTilePos.x, startTilePos.y,3);
        //check matches for the second piece
        var endMatches = GetMatchByPiece(endTilePos.x, endTilePos.y, 3);

        //store all the matches
        var allMatches = startMatches.Union(endMatches).ToList();



        if(allMatches.Count == 0)
        {
            /*Debug.Log("No match found, reverting pieces to original positions.");

            // Log positions before moving back
            Debug.Log($"Reverting StartPiece from: ({StartPiece.x}, {StartPiece.y}) to ({startTilePos.x}, {startTilePos.y})");
            Debug.Log($"Reverting EndPiece from: ({EndPiece.x}, {EndPiece.y}) to ({endTilePos.x}, {endTilePos.y})");*/


            StartPiece.MovePiece(startTilePos.x,startTilePos.y);
            EndPiece.MovePiece(endTilePos.x, endTilePos.y);

            activePieces[startTilePos.x, startTilePos.y] = StartPiece;
            activePieces[endTilePos.x,endTilePos.y]= EndPiece;

            //yield return new WaitForSeconds(0.6f); // Wait for the pieces to move back


            /*Debug.Log($"StartPiece reverted to: ({StartPiece.x}, {StartPiece.y})");
            Debug.Log($"EndPiece reverted to: ({EndPiece.x}, {EndPiece.y})");*/

        }
        else
        {
            ChangePieces(allMatches);
        }

        /*Debug.Log($"StartPiece final position: ({StartPiece.x}, {StartPiece.y})");
        Debug.Log($"EndPiece final position: ({EndPiece.x}, {EndPiece.y})");*/

        startTilePos = null;
        endTilePos = null;
        isSwapingPieces = false;    

        yield return null;

    }

    private void ChangePieces(List<Piece> piecesToClear)
    {
        piecesToClear.ForEach(piece =>
        {
            ClearPiece(piece.x, piece.y);

        });

        List<int> columnsToFill = GetColumnsToFill(piecesToClear);

       List<Piece> collapsedPieces = CollapseColumns(columnsToFill, 0.3f);

    }

    /// <summary>
    /// Move the pieces down after a match
    /// </summary>
    /// <param name="columnsToFill"></param>
    /// <param name="collapseTime"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private List<Piece> CollapseColumns(List<int> columnsToFill, float collapseTime)
    {
        List<Piece> collapsedPieces = new List<Piece>();
        
        for(int i = 0; i < columnsToFill.Count; i++)
        {
            var currentColumn = columnsToFill[i];

            for(int y = 0; y < gridHeight; y++)
            {
                if (activePieces[currentColumn, y] == null)
                {
                     for(int yPlus = y +1; yPlus < gridHeight; i++)
                    {
                        if(activePieces[currentColumn, yPlus] != null)
                        {
                            activePieces[currentColumn, yPlus].MovePiece(currentColumn, y);
                            activePieces[currentColumn, y] = activePieces[currentColumn,yPlus];

                            if (!collapsedPieces.Contains(activePieces[currentColumn, y]))
                            {
                                collapsedPieces.Add(activePieces[currentColumn, y]);
                            }

                            activePieces[currentColumn, yPlus] = null;
                            break;
                        }
                    }
                };
            }
        }

        return collapsedPieces;

    }

    /// <summary>
    /// Get list of columns where the pieces where eliminated
    /// </summary>
    /// <param name="piecesToClear"></param>
    /// <returns></returns>
    private List<int> GetColumnsToFill(List<Piece> piecesToClear)
    {
        var result = new List<int>();

        piecesToClear.ForEach(piece =>
        {
            if (!result.Contains(piece.x))
            {
                result.Add(piece.x);
            }
        });


        return result;
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
        if (Math.Abs((start.x - end.x)) == 1 && start.y == end.y)
        {
            return true;
        }
        //check if the destination of tile to move in Y is next to the current position 
        else if(Math.Abs((start.y - end.y)) == 1 && start.x == end.x)
        { 
            return true;
        }
        
        return false;  
    }

    /// <summary>
    /// Check if there are matches below or before
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <returns></returns>
    private bool HasPreviewsMatches(int xPos, int yPos)
    { 
        var downMatches = GetMatchByDirection(xPos, yPos, new Vector2(0,-1),2) ?? new List<Piece>();
        var leftMatches = GetMatchByDirection(xPos,yPos, new Vector2(-1,0),2) ?? new List<Piece>();

        return (downMatches.Count > 0 || leftMatches.Count > 0);

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
            nextX = xPos + ((int)direction.x * i) ;
            nextY = yPos + ((int)direction.y * i) ;

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
        var horizontalMatch = matchLeft.Union(matchRight).ToList();

        var totalMatches = new List<Piece>();

        if (verticalMatch.Count >= minPieces)
        {
            totalMatches = totalMatches.Union(verticalMatch).ToList();
        }

        if (horizontalMatch.Count >= minPieces)
        {
            totalMatches = totalMatches.Union(horizontalMatch).ToList();
        }

        return totalMatches;

    }


}




