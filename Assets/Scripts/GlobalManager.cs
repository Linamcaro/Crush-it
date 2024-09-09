using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public LevelManager levelManager;

    void Start()
    {
        // Load the first level
        levelManager.LoadLevel(1);
    }

    // Call this method when the player completes a level
    public void OnLevelComplete()
    {
        levelManager.LoadLevel(2); // Move to the next level
    }
}
