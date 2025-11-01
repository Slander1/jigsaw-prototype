using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.LvlConfigurator
{
    public class PiecesCountBtn : MonoBehaviour, IPointerClickHandler
    {
        public int PiecesCount { get; private set; }
        
        public event Action<PiecesCountBtn> Clicked;
        
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Image btnImage;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging) return;
            Clicked?.Invoke(this);
        }

        public void Init(int piecesCount)
        {
            PiecesCount = piecesCount;
            countText.text = PiecesCount.ToString();
        }
        
        public void SetSelected(bool isSelected)
        { 
            btnImage.color = isSelected ? Color.green : Color.gray;
        }
    }
}