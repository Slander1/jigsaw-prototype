using UnityEngine;
using UnityEngine.UI;

namespace UI.LvlConfigurator
{
    public class PuzzleFullResolutionPreview : MonoBehaviour
    {
        //TODO : позожая логика с PuzzlePreviewView
        public Sprite CashedSprite { get; set; }
        
        [SerializeField] private Image previewImage;

        public void Init(Sprite sprite)
        {
            CashedSprite = sprite;
            previewImage.sprite = sprite;
        }

        // public void OnPointerClick(PointerEventData eventData)
        // {
        //     if (eventData.dragging) return;
        //     Clicked?.Invoke(this);
        // }
    }
}