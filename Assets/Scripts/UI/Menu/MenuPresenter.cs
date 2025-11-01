using System;
using System.Collections.Generic;
using System.Linq;
using CoreLogic.Configs.Categories;
using CoreLogic.Configuration;
using CoreLogic.MVPPattern;
using Cysharp.Threading.Tasks;
using UI.LvlConfigurator;
using UI.Menu;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.Menu
{
    public sealed class MenuPresenter : Presenter, IDisposable
    {
        [SerializeField] private CategoryView categoryView;
        [SerializeField] private Transform categoryViewParent;
        
        [Space]
        [SerializeField] private PuzzlePreviewView puzzlePreviewView;
        [SerializeField] private Transform puzzlePreviewsParent;

        private readonly Dictionary<CategoryView, string> _createdCategoriesWithKeys = new();
        // private Dictionary<PuzzlePreviewView, UniTask<Sprite>> _puzzlePreviewViewsWithSprite = new();
        private List<PuzzlePreviewView> _createdPuzzlePreviewViews = new();
        
        private IConfigStorage _configStorage;
        private CoreLogic.Loader.ILoader _loader;
        private IModalsManager _modalsManager;

        private string _currentPickedCategoryKey;
        
        [Inject]
        private void Construct(IConfigStorage configStorage, CoreLogic.Loader.ILoader loader, IModalsManager modalsManager)
        {
            _configStorage = configStorage;
            _loader = loader;
            _modalsManager = modalsManager;
        }
        
        private void Start()
        {
            CreateMenuItems();
        }

        private void CreateMenuItems()
        {
            var categories = _configStorage.CategoriesList.categories;
            CreateCategories(categories);
            SetDefaultCategoryAsSelected(_configStorage.CategoriesList.categories[0].categoryKey);
        }

        private void CreateCategories(Category[] categories)
        {
            foreach (var category in categories)
            {
                var createdCategory = Instantiate(categoryView, categoryViewParent);
                createdCategory.SetTitle(category.categoryLabel);
                createdCategory.Clicked += OnCategorySelected;
                _createdCategoriesWithKeys.Add(createdCategory, category.categoryKey);
            }
        }

        private void SetDefaultCategoryAsSelected(string firstCategoryName)
        {
            var firstCategoryView = _createdCategoriesWithKeys.FirstOrDefault(x => x.Value == firstCategoryName).Key;
            OnCategorySelected(firstCategoryView);
        }

        private void OnCategorySelected(CategoryView selectedCategoryView)
        {
            foreach (var categoryWithKey in _createdCategoriesWithKeys)
            {
                var category = categoryWithKey.Key;
                category.SetSelected(false);
            
                if (selectedCategoryView != category) continue;
                _currentPickedCategoryKey = categoryWithKey.Value;
                var categoryData = _configStorage.CategoriesList.categories
                    .First(item => item.categoryKey == categoryWithKey.Value);
                ChangeCreatedPuzzlePreviewCount(categoryData);
                InitPreviews(categoryData).Forget();
                category.SetSelected(true);
            }
        }

        //TODO : могут быть утечки памяти в некоторых случаях
        private async UniTaskVoid InitPreviews(Category categoryData)
        {
            var categoryName = categoryData.categoryKey;
            var index = 0;
            
            foreach (var puzzleInCategory in categoryData.puzzlesData)
            {
                var sprite = await _loader.LoadSpriteAsync(true, categoryName, puzzleInCategory.id);
                var view = _createdPuzzlePreviewViews[index];
                ReleaseSprite(view);
                view.Init(sprite, puzzleInCategory);
                index++;
            }
        }

        private void ChangeCreatedPuzzlePreviewCount(Category categoryData)
        {
            var count = categoryData.puzzlesData.Length;
            
            while (_createdPuzzlePreviewViews.Count != count)
            {
                if (_createdPuzzlePreviewViews.Count > count)
                {
                    var lastItem = _createdPuzzlePreviewViews[^1];
                    _createdPuzzlePreviewViews.RemoveAt(_createdPuzzlePreviewViews.Count - 1);
                    lastItem.Clicked -= OnClickedPuzzlePreview;
                    
                    ReleaseSprite(lastItem);
                    Destroy(lastItem.gameObject);
            
                }
                else
                {
                    var createdItem = Instantiate(puzzlePreviewView, puzzlePreviewsParent);
                    _createdPuzzlePreviewViews.Add(createdItem);
                    createdItem.Clicked += OnClickedPuzzlePreview;
                }
            }
        }

        private static void ReleaseSprite(PuzzlePreviewView item)
        {
            if (item.CashedSprite == null) return;
            var sprite = item.CashedSprite;
            item.CashedSprite = null;
            Resources.UnloadAsset(sprite);
        }

        private void OnClickedPuzzlePreview(PuzzlePreviewView clickedPreview)
        {
            var lvlConfigurator = _modalsManager.Show<LvlConfiguratorPresenter>();
            lvlConfigurator.Init(_currentPickedCategoryKey, clickedPreview.Id);
        }

        public void Dispose()
        {
            foreach (var categoryWithKey in _createdCategoriesWithKeys)
            {
                var category = categoryWithKey.Key;
                category.Clicked -= OnCategorySelected;
            }

            foreach (var previewView in _createdPuzzlePreviewViews)
            {
                previewView.Clicked -= OnClickedPuzzlePreview;
                ReleaseSprite(previewView);
            }
            
            Destroy(categoryViewParent.gameObject);
            Destroy(puzzlePreviewsParent.gameObject);
        }
    }
}