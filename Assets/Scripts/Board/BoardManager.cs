using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private FoodCollectionSO foodCollection; // Reference to the FoodCollection


    /// <summary>
    /// Generate the food Piece
    /// </summary>
    /// <param name="position"></param>
    public void GenerateFoodPiece(Vector2 position)
    {
        GameObject piece = Instantiate(piecePrefab, position, Quaternion.identity);
        piece.transform.SetParent(transform); // Organize under parent for easier cleanup

        // Get a random food item from the collection
        int index = Random.Range(0, foodCollection.foodItems.Count);
        FoodItemSO randomFood = foodCollection.foodItems[index]; 
        piece.GetComponent<FoodPiece>().SetFood(randomFood.foodName, randomFood.foodSprite);
    }

    /// <summary>
    /// Generate the game board
    /// </summary>
    /// <param name="layout"></param>
    public void GenerateBoard(int[,] layout)
    {
        int width = layout.GetLength(0);
        int height = layout.GetLength(1);

        Vector2 offset = new Vector2((width - 1) / 2f, (height - 1) / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (layout[x, y] == 1) // Only place pieces in cells with a 1
                {
                    Vector3 position = new Vector2(x, y) - offset;

                    GenerateFoodPiece(position);

                }
            }
        }
    }
}
