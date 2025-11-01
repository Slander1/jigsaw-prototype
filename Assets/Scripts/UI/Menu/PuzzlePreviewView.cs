using System;
using CoreLogic.Configs.Categories;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public sealed class PuzzlePreviewView : MonoBehaviour, IPointerClickHandler
    {
        //TODO : позожая логика с PuzzleFullResolutionPreview
        
        public Sprite CashedSprite { get; set; }
        
        public int Id { get; private set; }
        
        //TODO : вынести в абстрактный класс с CategoryView
        public event Action<PuzzlePreviewView> Clicked;

        [SerializeField] private Image previewImage;
        [SerializeField] private TMP_Text mark;

        public void Init(Sprite sprite, PuzzleInCategory  puzzleInCategory)
        {
            CashedSprite = sprite;
            previewImage.sprite = sprite;
            Id = puzzleInCategory.id;

            if (puzzleInCategory.isPaid)
            {
                mark.gameObject.SetActive(true);
                mark.text = "paid";
            }
            else if (puzzleInCategory.isAd)
            {
                mark.gameObject.SetActive(true);
                mark.text = "ad";
            }
            else
            {
                mark.gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging) return;
            Clicked?.Invoke(this);
        }
    }
}