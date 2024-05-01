using System.Collections.Generic;
using System.Threading.Tasks;
using Categories;
using Sketches.Services;
using UI.Services;
using UnityEngine;
using Zenject;


namespace UI.Controller
{
    public class MainUIController : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private GameObject categoryPanel;
        [SerializeField] private GameObject mainPagePanel;


        [Space, Header("Category List")] [SerializeField]
        private GameObject categoryParentObject;

        [SerializeField] private GameObject categoryItemPrefab;


        private bool isFetchingDone = false;
        private List<Category> _categories;


        [Inject] private readonly FetchCategoriesService _fetchCategoriesService;

        private async void Start()
        {
            // Fetch Categories!
            await fetchCategories();
        }


        async Task fetchCategories()
        {
            _categories = await _fetchCategoriesService.FetchCategoryList();

            await Task.Yield();

            isFetchingDone = true;

            await SetUpCategoryItems(_categories);
        }


        private async Task SetUpCategoryItems(List<Category> categories)
        {
            foreach (Category cat in categories)
            {
                GameObject categoryObject = Instantiate(categoryItemPrefab, categoryParentObject.transform);
                CategoryController controller = categoryObject.GetComponent<CategoryController>();
                controller.SetText(cat.Name);
                await controller.SetImageFromUrl(cat.CoverImageUrl);
            }
        }

        //private IEnumerator LoadImage

        public void ShowCategoryPanel()
        {
            mainPagePanel.SetActive(false);
            categoryPanel.SetActive(true);
        }


        public void ShowMainPage()
        {
            categoryPanel.SetActive(false);
            mainPagePanel.SetActive(true);
        }
    }
}