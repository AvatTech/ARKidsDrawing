using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Repositories;
using Sketches.Services;
using UI.Services;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<CategoryRepository>().AsSingle();


        Container.Bind<FetchCategoriesService>().AsTransient();
        Container.Bind<FirestoreCategoryManager>().AsTransient();
        //Container.Bind<ImageLoaderService>().AsTransient().NonLazy();
    }
}