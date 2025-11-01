using System;
using UnityEngine;

namespace CoreLogic.Configs.Categories
{
    /// <summary>
    /// Самое простое представление картинки через id и имя.
    /// </summary>
    [Serializable]
    public sealed class Category
    {
        [SerializeField] public string categoryLabel;
        [SerializeField] public string categoryKey;
        [SerializeField] public PuzzleInCategory[] puzzlesData;
    }

    [Serializable]
    public class PuzzleInCategory
    {
        public int id;
        public bool isPaid;
        public bool isAd;
    }
}