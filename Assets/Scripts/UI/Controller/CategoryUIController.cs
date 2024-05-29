using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Controller;
using Categories.Model;
using Categories.Services;
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

        [SerializeField] private SplashScreenController _splashScreenController;

        private List<Category> _categories = new();

        [Inject] private readonly FetchCategoriesService _fetchCategoriesService;


        private async void OnEnable()
        {
            await SyncCategories();
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
            if (!ConnectionChecker.IsNetworkChecked)
            {
                Debug.Log("Network checkinng..");
                _splashScreenController.SetState(SplashScreenState.Loading);

                // Check connection
                if (!await CheckConnection())
                {
                    Debug.Log("we dont have internet * *");
                    return;
                }
                Debug.Log("we have internet * *");
            }


            // Get categories list
            _categories = await _fetchCategoriesService.FetchCategoryList();

            // setup category items in the scene
            await SetUpCategoryItems(_categories);

            _splashScreenController.SetState(SplashScreenState.Done);
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

            _splashScreenController.SetState(SplashScreenState.Done);
        }


        private async Task<bool> CheckConnection()
        {
            if (!await ConnectionChecker.IsConnectedToNetwork())
            {
                Debug.Log("kheyli bad...");
                SplashScreenController.Instance.SetState(SplashScreenState.Failed);
                return false;
            }
            else
            {
                ConnectionChecker.IsNetworkChecked = true;
                return true;
            }
            
            
        }
    }
}