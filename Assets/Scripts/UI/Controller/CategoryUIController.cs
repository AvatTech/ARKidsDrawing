using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Model;
using Categories.Services;
using UnityEngine;
using Zenject;

namespace UI.Controller
{
    public class CategoryUIController : MonoBehaviour
    {
        [Space, Header("Category List")] [SerializeField, Tooltip("Where the category objects will instantiated?")]
        private GameObject categoryParentObject;

        [SerializeField, Tooltip("Category Item which has Category Controller.")]
        private GameObject categoryItemPrefab;

        private List<Category> _categories;

        [Inject] private readonly FetchCategoriesService _fetchCategoriesService;

        private void OnEnable()
        {
            SyncCategories();
        }


        public void SyncCategories()
        {
            SplashScreenController.Instance.SetState(SplashScreenState.Loading);

            // Fetch Categories!

            Task fetchTask = FetchCategories();
            Task timerTask = Task.Delay(5000);

            fetchTask.Start();
            timerTask.Start();

            while (!timerTask.IsCompleted)
            {
                if (fetchTask.IsCompleted || fetchTask.IsCompletedSuccessfully)
                {
                    SplashScreenController.Instance.SetState(SplashScreenState.Done);
                    break;
                }

                if (fetchTask.IsCanceled || fetchTask.IsFaulted)
                {
                    SplashScreenController.Instance.SetState(SplashScreenState.Failed);
                    break;
                }
            }

            SplashScreenController.Instance.SetState(SplashScreenState.Failed);
        }

        async Task FetchCategories()
        {
            _categories = await _fetchCategoriesService.FetchCategoryList();

            await Task.Yield();


            await SetUpCategoryItems(_categories);
        }


        private async Task SetUpCategoryItems(List<Category> categories)
        {
            foreach (Category cat in categories)
            {
                GameObject categoryObject = Instantiate(categoryItemPrefab, categoryParentObject.transform);
                CategoryController controller = categoryObject.GetComponent<CategoryController>();
                controller.Category = cat;
                controller.SetText(cat.Name);
                await controller.SetImageFromUrl(cat.CoverImageUrl);
            }

            await Task.Yield();
        }
    }
}