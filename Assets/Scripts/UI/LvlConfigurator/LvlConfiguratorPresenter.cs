using System;
using System.Collections.Generic;
using System.Linq;
using CoreLogic.Configuration;
using CoreLogic.Loader;
using CoreLogic.MVPPattern;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI.LvlConfigurator
{
    public sealed class LvlConfiguratorPresenter : Presenter, IDisposable
    {
        [SerializeField] private PuzzleFullResolutionPreview puzzleFullResolutionPreview;
        [SerializeField] private CloseLastPresenterButton closeBtn;
        
        [Space]
        [SerializeField] private PiecesCountBtn piecesDifficultBtn;
        [SerializeField] private Transform piecesDifficultLvlParent;

        [Space]
        [SerializeField] private PlayButton playButton;
        
        private readonly List<PiecesCountBtn> _piecesDifficultBtns = new ();
        
        private DiContainer _diContainer;
        private ILoader _loader;
        private IConfigStorage _configStorage;
        
        [Inject]
        public void Construct(ILoader loader, IConfigStorage configStorage, DiContainer diContainer)
        {
            _loader = loader;
            _diContainer = diContainer;
            _configStorage = configStorage;
            
            diContainer.Inject(closeBtn);
        }
        
        public void Init(string categoryKey, int imageId)
        {
            LoadFullResolutionSprite(categoryKey, imageId).Forget();
            InitDifficultButtons();
            InitPlayBtn(categoryKey, imageId);
        }

        private void InitPlayBtn(string categoryKey, int imageId)
        {
            var category = _configStorage.CategoriesList.categories.First(item => item.categoryKey == categoryKey);
            var puzzleData = category.puzzlesData.First(item => item.id == imageId);

            var initedString = "Play free";
            if (puzzleData.isAd)
            {
                initedString = "Play after ad";
            }
            else if (puzzleData.isPaid)
            {
                initedString = "Play for gold";
            }
            playButton.Init(initedString);
        }

        private void InitDifficultButtons()
        {
            var piecesDifficultLevels = _configStorage.PiecesDifficultLvl;

            foreach (var count in piecesDifficultLevels.piecesCount)
            {
                var createdBtn = Instantiate(piecesDifficultBtn,  piecesDifficultLvlParent);
                createdBtn.Init(count);
                _piecesDifficultBtns.Add(createdBtn);
                createdBtn.Clicked += OnDifficultBtnClicked;
            }
            
            OnDifficultBtnClicked(_piecesDifficultBtns[0]);
        }

        private void OnDifficultBtnClicked(PiecesCountBtn piecesCountBtn)
        {
            foreach (var difficultBtn in _piecesDifficultBtns)
            {
                difficultBtn.SetSelected(piecesCountBtn == difficultBtn);
            }
        }
        
        //TODO: возможны утечки памяти
        private async UniTaskVoid LoadFullResolutionSprite(string categoryKey, int imageId)
        {
            var sprite = await _loader.LoadSpriteAsync(false, categoryKey, imageId);
            
            // var sprite = LoadFullResolutionSprite();
            puzzleFullResolutionPreview.Init(sprite);
        }
        
        private static void ReleaseSprite(PuzzleFullResolutionPreview item)
        {
            if (item.CashedSprite == null) return;
            var sprite = item.CashedSprite;
            item.CashedSprite = null;
            Resources.UnloadAsset(sprite);
        }

        public void Dispose()
        {
            foreach (var difficultBtn in _piecesDifficultBtns) difficultBtn.Clicked -= OnDifficultBtnClicked;

            ReleaseSprite(puzzleFullResolutionPreview);
        }
    }
}