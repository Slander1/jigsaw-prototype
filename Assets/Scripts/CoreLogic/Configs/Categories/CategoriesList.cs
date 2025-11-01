using System;
using UnityEngine;

namespace CoreLogic.Configs.Categories
{
    [CreateAssetMenu(
        fileName = "CategoriesList",
        menuName = "Configs/Categories List",
        order = 0)]
    
    [Serializable]
    public sealed class CategoriesList : ScriptableObject
    {
        [SerializeField] public Category[] categories;
        
        /// <summary>
        /// Скрипт для изначального заполнения конфига
        /// </summary>
        
        private void OnValidate()
        {
            foreach (var category in categories)
            {
                var id = 1;
                
                foreach (var puzzleData in category.puzzlesData)
                {
                    puzzleData.id = id;
                    id++;
                }
            }
        }
    }
    
}