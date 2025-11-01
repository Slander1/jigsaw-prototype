using CoreLogic.Configuration;
using CoreLogic.Loader;
using CoreLogic.MVPPattern;
using Zenject;

namespace CoreLogic.Installers
{

    public class ProjectCoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<IConfigStorage>().To<MockConfigStorage>().AsSingle();
            Container.Bind<ILoader>().To<ResourcesLoader>().AsSingle();
            Container.Bind<IModalsManager>().To<ModalsManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<AppBootstrap>().AsSingle().NonLazy();
        }
    }
    
}