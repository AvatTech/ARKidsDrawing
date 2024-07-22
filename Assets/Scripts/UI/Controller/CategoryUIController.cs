using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvatAdmobExtension.Script.Manager;
using Categories.Controller;
using Categories.Model;
using Categories.Services;
using Network;
using Story.Controller;
using Story.Manager;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace UI.Controller
{
    public class CategoryUIController : MonoBehaviour
    {
        [Space, Header("Category List")] [SerializeField, Tooltip("Where the category objects will instantiated?")]
        private GameObject categoryParentObject;

        [SerializeField, Tooltip("Category Item which has Category Controller.")]
        private GameObject categoryItemPrefab;

        [SerializeField] private LoadingController loadingController;

        [Space, SerializeField] private UnityEngine.UI.Button tryAgainButton;

        private List<Category> _categories = new();

        [Inject] private readonly FetchCategoriesService _fetchCategoriesService;

        [Inject] private readonly StoryManager _storyManager;

        [Inject] private ReviewManager _reviewManager;

        private async void Start()
        {
            // var storyPanel = GetComponent<StoryPanelController>();
            // _storyManager.storyPanelControllers.Add(storyPanel);

            tryAgainButton.onClick.AddListener(OnTryAgainClicked);
            await SyncCategories();
        }


        private void OnEnable()
        {
            StartCoroutine(AdManager.Instance.BannerAd.LoadThenShow());
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
            // Get categories list
            _categories = await _fetchCategoriesService.FetchCategoryList(loadingController);

            if (_categories is null)
                return;


            // if (!ConnectionChecker.IsNetworkChecked)
            // {
            //     Debug.Log("Network checking..");
            //     loadingController.SetState(SplashScreenState.Loading);
            //
            //     // Check connection
            //     if (!await CheckConnection())
            //     {
            //         Debug.Log("we dont have internet * *");
            //         return;
            //     }
            //
            //     Debug.Log("we have internet * *");
            // }


            // setup category items in the scene
            await SetUpCategoryItems(_categories);

            loadingController.SetState(SplashScreenState.Done);
        }


        private async Task SetUpCategoryItems(List<Category> categories)
        {
            Debug.Log($"Category null: {_categories == null}");

            foreach (Category cat in categories)
            {
                GameObject categoryObject = Instantiate(categoryItemPrefab, categoryParentObject.transform);
                CategoryController controller = categoryObject.GetComponent<CategoryController>();
                controller.Category = cat;
                controller.SetText(cat.Name);
                await controller.SetImageFromUrl(cat.CoverImageUrl, exception => throw exception);
            }

            loadingController.SetState(SplashScreenState.Done);

            StartCoroutine(ReviewRoutine());
        }


        // private async Task<bool> CheckConnection()
        // {
        //     if (!await ConnectionChecker.IsConnectedToNetwork())
        //     {
        //         Debug.Log("Kheili bad...");
        //         LoadingController.Instance.SetState(SplashScreenState.Failed);
        //         return false;
        //     }
        //     else
        //     {
        //         ConnectionChecker.IsNetworkChecked = true;
        //         return true;
        //     }
        // }

        private IEnumerator ReviewRoutine()
        {
            yield return new WaitForSeconds(2f);

            _reviewManager.CheckReview();
        }
    }
}