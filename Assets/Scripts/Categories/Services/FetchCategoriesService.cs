using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Model;
using Firebase.Firestore;
using Loading;
using Network;
using Repositories;
using Sketches.Model;
using Sketches.Services;
using Storage;
using UI.Controller;
using Unity.Services.Core;
using Zenject;
using LoadingController = UI.Controller.LoadingController;

namespace Categories.Services
{
    public class FetchCategoriesService
    {
        [Inject] private readonly CategoryRepository _categoryRepository;
        [Inject] private readonly FetchSketchesService _fetchSketchesService;
        [Inject] private readonly ILocalStorage _localStorage;


        /// <summary>
        /// Load list of categories from cache
        /// If cache is empty, return null
        /// </summary>
        /// <returns></returns>
        public List<Category> LoadCategoriesFromCache()
        {
            // load data from storage
            var storageCategories = _localStorage.LoadCategory();

            return storageCategories;
        }


        /// <summary>
        /// Load list of categories from firestore online
        /// </summary>
        /// <returns></returns>
        public async Task<List<Category>> LoadCategoriesFromFirestore()
        {
            Task<QuerySnapshot> thisTask = null;

            // get data from firebase and save it
            await _categoryRepository.GetCategories().ContinueWith(task => { thisTask = task; });

            var data = await OnQueryReceived(thisTask.Result);

            _localStorage.SaveCategory(data);


            return data;
        }

        public async Task<List<Category>> FetchCategoryList(LoadingController loadingController)
        {
            // first load by cache
            if (LoadCategoriesFromCache() != null)
                return LoadCategoriesFromCache();

            // show loading page
            loadingController.SetState(SplashScreenState.Loading);

            // checking connection
            bool isConnected = await ConnectionChecker.IsConnectedToNetwork();

            // if connected, get data from firestore
            if (isConnected)
            {
                var fetchedData = await LoadCategoriesFromFirestore();
                //loadingController.SetState(SplashScreenState.Done);
                return fetchedData;
            }
            else
            {
                loadingController.SetState(SplashScreenState.Failed);
                return null;
            }
        }


        /// <summary>
        /// Convert received query snapshot to list of categories.
        /// Create category class for each
        /// </summary>
        /// <param name="querySnapshot"></param>
        /// <returns></returns>
        private async Task<List<Category>> OnQueryReceived(QuerySnapshot querySnapshot)
        {
            List<Category> _categories = new();


            foreach (var doc in querySnapshot.Documents)
            {
                var dic = doc.ToDictionary();

                dic.TryGetValue("name", out var categoryName);
                dic.TryGetValue("coverImage", out var categoryCoverImage);


                Category newCat = new Category(
                    name: categoryName.ToString(),
                    id: doc.Id,
                    coverImageUrl: categoryCoverImage.ToString()
                );

                // Fetch sketches of this category
                var currentCategorySketches = await GetSketchesById(newCat.Id);

                // Add sketches to the List
                foreach (Sketch sketch in currentCategorySketches)
                {
                    newCat.Sketches.Add(sketch);
                }

                _categories.Add(newCat);
            }


            return _categories;
        }

        private async Task<List<Sketch>> GetSketchesById(string categoryId)
        {
            return await _fetchSketchesService.FetchSketchesById(categoryId);
        }
    }
}