using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Categories.Controller;
using Categories.Model;
using Categories.Services;
using Extensions.Unity.ImageLoader;
using Network;
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


        private async void OnEnable()
        {
            await SyncCategories();
        }

        private void OnDisable()
        {
            //ImageLoader.ClearCache();
        }

        public async void OnTryAgainClicked()
        {
            await SyncCategories();
        }

        public async Task SyncCategories()
        {
            // Fetch Categories!
            await FetchCategories();
        }


        private async Task FetchCategories()
        {
            SplashScreenController.Instance.SetState(SplashScreenState.Loading);

            // Check connection
            if(!await CheckConnection())
                return;
                

            // Get categories list
            _categories = await _fetchCategoriesService.FetchCategoryList();

            // setup category items in the scene
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
                await controller.SetImageFromUrl(cat.CoverImageUrl, exception => throw exception);
            }

            SplashScreenController.Instance.SetState(SplashScreenState.Done);

            categories.Clear();

            await Task.Yield();
        }


        private async Task<bool> CheckConnection()
        {
            if (!await ConnectionChecker.IsConnectedToNetwork())
            {
                SplashScreenController.Instance.SetState(SplashScreenState.Failed);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}