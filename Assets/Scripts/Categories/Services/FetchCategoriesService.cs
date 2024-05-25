using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Model;
using Firebase.Firestore;
using Repositories;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace Categories.Services
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

                _categories.Add(newCat);
            }
        }


        public async Task<List<Category>> FetchCategoryList()
        {
            await _categoryRepository.GetCategories().ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                    throw new RequestFailedException(0, "Firebase request has been failed!");
                else
                    OnQueryReceived(task.Result);
            });

            

            return _categories;
        }
    }
}