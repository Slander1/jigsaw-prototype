using System;
using System.Collections.Generic;
using System.Linq;
using CoreLogic.Configs.Modals;
using UnityEngine;
using Zenject;

namespace CoreLogic.MVPPattern
{
    public interface IModalsManager
    {
        public T Show<T>() where T : Presenter;
        public bool TryShow<T>(out T presenter) where T : Presenter;
        public void CloseLastPresenter();
    }
    
    public sealed class ModalsManager : IModalsManager, IDisposable
    {
        private Transform _presentersParent;
        
        private List<Presenter> openedPresenters = new();
        private readonly Dictionary<Type, Presenter> _modalPrefabs;
        private readonly DiContainer _diContainer;

        public ModalsManager(IPresentersConfigInstaller  presentersConfigInstaller, DiContainer diContainer)
        {
            _presentersParent = UnityEngine.Object.Instantiate(new  GameObject("PresentersParent")).transform;
            
            _modalPrefabs = presentersConfigInstaller.PresentersPrefabs.ToDictionary
                (presenter => presenter.GetType(), presenter => presenter);
            _diContainer = diContainer;
        }
        
        public T Show<T>() where T : Presenter
        {
            var createdPresenter = UnityEngine.Object.Instantiate(_modalPrefabs[typeof(T)], _presentersParent);
            openedPresenters.Add(createdPresenter);
            createdPresenter.Closed += OnClosed;
            _diContainer.Inject(createdPresenter);
            
            return (T) createdPresenter;
        }

        public bool TryShow<T>(out T presenter) where T : Presenter
        {
            if (_modalPrefabs.TryGetValue(typeof(T), out Presenter value))
            {
                presenter = (T) value;
                return true;
            }

            presenter = null;
            
            return false;
        }

        public void CloseLastPresenter()
        {
            OnClosed(openedPresenters[^1]);
        }

        private void OnClosed(Presenter presenter)
        {
            presenter.Closed -= OnClosed;
            openedPresenters.Remove(presenter);
            UnityEngine.Object.Destroy(presenter.gameObject);
        }

        public void Dispose()
        {
            foreach (var presenter in openedPresenters) 
                presenter.Closed -= OnClosed;
            
            UnityEngine.Object.Destroy(_presentersParent.gameObject);
        }
    }
}