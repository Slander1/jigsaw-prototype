using System;
using CoreLogic.Loader;
using CoreLogic.MVPPattern;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.LvlConfigurator
{
    public sealed class CloseLastPresenterButton : MonoBehaviour, IPointerClickHandler
    {
        // public event Action Clicked;

        private IModalsManager _modalsManager;
        
        [Inject]
        public void Construct(IModalsManager modalsManager)
        {
            _modalsManager = modalsManager;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging) return;
            _modalsManager.CloseLastPresenter();
            // Clicked?.Invoke();
        }
    }
}