using System;
using CoreLogic.MVPPattern;
using UnityEngine;
using Zenject;

namespace CoreLogic.Configs.Modals
{
    
    public interface IPresentersConfigInstaller
    {
        public Presenter[] PresentersPrefabs { get; }
    }
    
    [CreateAssetMenu(
        fileName = "ModalsInstaller",
        menuName = "Installers/Modals Installer",
        order = 0)]
    
    [Serializable]
    public sealed class PresentersConfigInstaller : ScriptableObjectInstaller<PresentersConfigInstaller>, IPresentersConfigInstaller
    {
        [SerializeField] public Presenter[] presentersPrefabs;
        public Presenter[] PresentersPrefabs => presentersPrefabs;
        
        public override void InstallBindings()
        {
            Container.Bind<IPresentersConfigInstaller>().FromInstance(this).AsSingle();
        }
    }
}