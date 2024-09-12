using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private FoodCollectionSO foodCollection; // Reference to the FoodCollection

    private FoodPiece[,] activePieces;
    private bool isSwapingPieces = false;

    private Vector2 offset;

    /// <summary>
    /// Generate the food Piece
    /// </summary>
    /// <param name="position"></param>
    public void GenerateFoodPiece(int x, int y, Vector2 offset)
    {

        Vector2 position = new Vector2(x, y) - offset;

        var piece = Instantiate(piecePrefab, position, Quaternion.identity);
        piece.transform.SetParent(transform); // Organize under parent for easier cleanup

        // Get a random food item from the collection
        int index = Random.Range(0, foodCollection.foodItems.Count);
        FoodItemSO randomFood = foodCollection.foodItems[index]; 

        activePieces[x, y] = piece.GetComponent<FoodPiece>();
        activePieces[x,y]?.SetFood(randomFood.foodName, randomFood.foodSprite, x, y);

    }

    /// <summary>
    /// Generate the game board
    /// </summary>
    /// <param name="layout"></param>
    public void GenerateBoard(int[,] layout)
    {
       int width = layout.GetLength(0);
       int height = layout.GetLength(1);

        activePieces = new FoodPiece[width, height];

        offset = new Vector2((width - 1) / 2f, (height - 1) / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (layout[x, y] == 1) // Only place pieces in cells with a 1
                {
                   // Vector3 position = new Vector2(x, y) - offset;

                     GenerateFoodPiece(x, y, offset);
                }
            }
        }
    }


    public void SwapTiles(FoodPiece startTilePos, FoodPiece endTilePos)
    {
        if (!isSwapingPieces)
        {
            StartCoroutine(SwapTilesIE(startTilePos, endTilePos));
        }
    }

    /// <summary>
    /// Swap the tile to the new position
    /// </summary>
    IEnumerator SwapTilesIE(FoodPiece startTilePos, FoodPiece endTilePos)
    {
        isSwapingPieces = true;

        //Save refence of the pieces in the two positions
        var StartPiece = activePieces[startTilePos.x, startTilePos.y];
        var EndPiece = activePieces[endTilePos.x, endTilePos.y];

        //Move the pieces to the new positions
        StartPiece.MovePiece(endTilePos.x, endTilePos.y, offset);
        EndPiece.MovePiece(startTilePos.x, startTilePos.y, offset);

        //update the coordinates in the activePieces array
        activePieces[startTilePos.x, startTilePos.y] = EndPiece;
        activePieces[endTilePos.x, endTilePos.y] = StartPiece;

        yield return new WaitForSeconds(0.6f);

        /* //check matches for the first piece
         var startMatches = GetMatchByPiece(startTilePos.x, startTilePos.y, 3);
         //check matches for the second piece
         var endMatches = GetMatchByPiece(endTilePos.x, endTilePos.y, 3);

         //store all the matches
         var allMatches = startMatches.Union(endMatches).ToList();



         if (allMatches.Count == 0)
         {


             StartPiece.MovePiece(startTilePos.x, startTilePos.y);
             EndPiece.MovePiece(endTilePos.x, endTilePos.y);

             activePieces[startTilePos.x, startTilePos.y] = StartPiece;
             activePieces[endTilePos.x, endTilePos.y] = EndPiece;
             isSwapingPieces = false;

         }
         else
         {
             ChangePieces(allMatches);
             AwardPoints(allMatches);
         }

         startTilePos = null;
         endTilePos = null;*/

        isSwapingPieces = false;
        yield return null;

    }

}
