﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Model;
using Firebase.Firestore;
using Repositories;
using Sketches.Model;
using Sketches.Services;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace Categories.Services
{
    public class FetchCategoriesService
    {
        [Inject] private readonly CategoryRepository _categoryRepository;
        [Inject] private readonly FetchSketchesService _fetchSketchesService;


        public async Task<List<Category>> FetchCategoryList()
        {
            Task<QuerySnapshot> currentTask = null;

            await _categoryRepository.GetCategories().ContinueWith(task =>
            {
                currentTask = task;
                if (task.IsFaulted || task.IsCanceled)
                    throw new RequestFailedException(0, "Firebase request has been failed!");
            });

            return await OnQueryReceived(currentTask?.Result);
        }

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