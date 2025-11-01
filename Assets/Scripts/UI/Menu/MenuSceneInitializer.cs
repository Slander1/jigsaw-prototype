using CoreLogic.MVPPattern;
using UI.MainMenu.Menu;
using UnityEngine;
using Zenject;

namespace UI.Menu
{
    public class MenuSceneInitializer : MonoBehaviour
    {
        [Inject]
        public void Construct(IModalsManager modalsManager, DiContainer diContainer)
        {
            modalsManager.Show<MenuPresenter>();
        }
    }
}