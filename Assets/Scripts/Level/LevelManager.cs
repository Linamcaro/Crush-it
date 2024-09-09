using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BoardManager boardManager; // Reference to the BoardManager
  
    // Method to load a specific level
    public void LoadLevel(int level)
    {
        int[,] layout = null;

        switch (level)
        {
            case 1:
                layout = LevelLayouts.level1Layout;
                break;
            case 2:
                layout = LevelLayouts.level2Layout;
                break;
                // Add more cases for additional levels
        }

        if (layout != null)
        {
            boardManager.GenerateBoard(layout); // Pass the layout to the BoardManager
        }
        else
        {
            Debug.LogError("Level layout not found!");
        }
    }
}
