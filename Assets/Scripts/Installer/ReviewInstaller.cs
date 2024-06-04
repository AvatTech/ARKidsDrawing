using Zenject;

namespace Installer
{
    public class ReviewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ReviewManager>().AsSingle();
        }
    }
}