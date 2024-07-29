using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ListOfPieces", menuName = "SO/ListOfPieces")]
public class PiecesSO : ScriptableObject
{
    public GameObject[] pieces;
}
