using CoreLogic.Configuration;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CoreLogic
{
    public sealed class AppBootstrap : IInitializable
    {
        private const string MenuScene = "Menu";
        
        private readonly IConfigStorage _storage;
        private readonly ZenjectSceneLoader _sceneLoader;
        
        public AppBootstrap(IConfigStorage storage, ZenjectSceneLoader sceneLoader)
        {
            _storage = storage; _sceneLoader = sceneLoader;
        }
        
        public void Initialize() => Run().Forget();
        
        private async UniTaskVoid Run()
        {
            await _storage.LoadCategoriesConfig();
            await _storage.LoadDifficultConfig();
            await _sceneLoader.LoadSceneAsync(MenuScene).ToUniTask();
        }
    }
}