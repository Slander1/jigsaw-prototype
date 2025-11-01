using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreLogic.Configs.PiecesCount
{

    [CreateAssetMenu(
        fileName = "PiecesDifficultLevels",
        menuName = "Configs/PiecesDifficultLevels",
        order = 0)]
    
    [Serializable]
    public sealed class PiecesDifficultLevels : ScriptableObject
    {
        [SerializeField] public List<int> piecesCount;
    }
}