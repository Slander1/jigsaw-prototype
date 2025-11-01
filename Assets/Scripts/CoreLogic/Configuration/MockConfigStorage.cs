using System;
using CoreLogic.Configs.Categories;
using CoreLogic.Configs.PiecesCount;
using CoreLogic.Loader;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreLogic.Configuration
{
    public sealed class MockConfigStorage : IConfigStorage
    {
        public CategoriesList CategoriesList { get; private set; }
        public PiecesDifficultLevels PiecesDifficultLvl { get; private set; }
        
        private const string CategoriesPath = "Configs/CategoriesList";
        private const string DifficultPath = "Configs/PiecesDifficultLevels";

        [Inject] private ILoader _loader;
        
        public async UniTask LoadCategoriesConfig()
        {
            var categoryList = await _loader.LoadScriptableConfigAsync<CategoriesList>(CategoriesPath);
           
            if (categoryList == null)
                throw new NullReferenceException($"[ResourcesLoader] Не удалось загрузить CategoriesList по пути: {CategoriesPath}");

            CategoriesList =  categoryList;
        }
        
        public async UniTask LoadDifficultConfig()
        {
            var difficultLvl = await _loader.LoadScriptableConfigAsync<PiecesDifficultLevels>(DifficultPath);
           
            if (difficultLvl == null)
                throw new NullReferenceException($"[ResourcesLoader] Не удалось загрузить CategoriesList по пути: {CategoriesPath}");

            PiecesDifficultLvl =  difficultLvl;
        }
    }
}