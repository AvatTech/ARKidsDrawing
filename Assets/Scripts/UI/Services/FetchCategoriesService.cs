using System.Collections.Generic;
using System.Threading.Tasks;
using Categories;
using Firebase.Firestore;
using Repositories;
using UnityEngine;
using Zenject;

namespace UI.Services
{
    public class FetchCategoriesService
    {
        [Inject] private readonly CategoryRepository _categoryRepository;

        private List<Category> _categories = new();


        private void OnQueryReceived(QuerySnapshot querySnapshot)
        {
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

                Debug.Log(newCat.Name);

                _categories.Add(newCat);
            }
        }


        public async Task<List<Category>> FetchCategoryList()
        {
            Debug.Log("Getting categories!");
            QuerySnapshot snapshot = await _categoryRepository.GetCategories();
            OnQueryReceived(snapshot);

            await Task.Yield();

            Debug.Log("Done!");

            return _categories;
        }
    }
}