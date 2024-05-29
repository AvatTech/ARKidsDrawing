using Categories.Services;
using Firebase.Firestore;
using Network;
using Repositories;
using Sketches.Builder;
using Sketches.Services;
using Storage;
using Zenject;

namespace Installer
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallStorages();
            InstallRepositories();
            InstallServices();
            InstallFirestore();
        }

        private void InstallStorages()
        {
            Container.BindInterfacesTo<LocalStorage>().AsSingle();
        }

        private void InstallRepositories()
        {
            Container.Bind<CategoryRepository>().AsSingle();
            Container.Bind<SketchRepository>().AsSingle();
        }

        private void InstallServices()
        {
            Container.Bind<FetchCategoriesService>().AsSingle();
            Container.Bind<FetchSketchesService>().AsSingle();
        }

        private void InstallFirestore()
        {
            Container.Bind<FirestoreCategoryManager>().AsSingle();
            Container.Bind<FirestoreSketchManager>().AsSingle();
        }
    }
}