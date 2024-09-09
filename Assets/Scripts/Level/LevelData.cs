using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData 
{
        public int[,] layout; // 0: Empty, 1: Normal piece

        public LevelData(int[,] layout)
        {
            this.layout = layout;
        }
    
}
