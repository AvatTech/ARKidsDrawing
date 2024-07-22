using Firebase.RemoteConfig;
using Repositories;
using Services;
using Zenject;

namespace Installer
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ReviewManager>().AsSingle();
            Container.Bind<IAPService>().AsSingle();
            Container.Bind<IAPRepository>().AsSingle();
            Container.Bind<RemoteConfig>().AsSingle().NonLazy();
        }
    }
}