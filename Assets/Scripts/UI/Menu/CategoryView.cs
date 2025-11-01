using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class CategoryView : MonoBehaviour, IPointerClickHandler
    {
        //TODO : вынести в абстрактный класс с PuzzlePreviewView
        public event Action<CategoryView> Clicked;
        
        [SerializeField] private TMP_Text label;
        
        public string _rawTitle;

        public void SetTitle(string title)
        {
            _rawTitle = title;
            label.text = _rawTitle;
        }

        public void SetSelected(bool isSelected)
        {
            // View — только форматирует отображение по сигналу презентера
            label.text = isSelected ? $"<u>{_rawTitle}</u>" : _rawTitle;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging) return;
            Clicked?.Invoke(this);
        }
    }
}