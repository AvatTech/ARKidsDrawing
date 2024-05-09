using Categories.Services;
using Firebase.Firestore;
using Repositories;
using Sketches.Builder;
using Sketches.Services;
using Zenject;

namespace Installer
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {

            InstallRepositories();
            InstallServices();
            InstallFirestore();
        }


        private void InstallRepositories()
        {
            Container.Bind<CategoryRepository>().AsSingle();
            Container.Bind<SketchRepository>().AsSingle();
        }

        private void InstallServices()
        {
            Container.Bind<FetchCategoriesService>().AsTransient();
            Container.Bind<FetchSketchesService>().AsTransient();
        }

        private void InstallFirestore()
        {
            Container.Bind<FirestoreCategoryManager>().AsTransient();
            Container.Bind<FirestoreSketchManager>().AsTransient();
        }
    }
}