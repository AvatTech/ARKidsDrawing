using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Model;
using Categories.Services;
using Categories.Utills;
using Sketches.Controller;
using Sketches.Model;
using Sketches.Services;
using UnityEngine;
using Zenject;

namespace UI.Controller
{
    public class SketchUIController : MonoBehaviour
    {
        [SerializeField] private GameObject sketchPrefab;
        [SerializeField] private GameObject sketchParentObject;

        [Inject] private readonly FetchSketchesService _fetchSketchesService;
        private CurrentCategoryManager _currentCategoryManager;

        private List<SketchController> currentSketches = new();

        private List<Sketch> _sketches = new();

        private void Awake()
        {
            _currentCategoryManager = CurrentCategoryManager.Instance;
        }

        private async void OnEnable()
        {
            if (_currentCategoryManager.CurrentCategory == null)
                return;


            // Fetch sketches!
            await FetchSketches();
        }

        private void OnDisable()
        {
            RemoveSketches();
        }

        private async Task FetchSketches()
        {
            _sketches = await _fetchSketchesService.FetchSketchesById(_currentCategoryManager.CurrentCategory.Id);

            await Task.Yield();


            await SetUpSketchItems(_sketches);
        }


        private async Task SetUpSketchItems(List<Sketch> sketches)
        {
            foreach (Sketch sketch in sketches)
            {
                GameObject sketchObj = Instantiate(sketchPrefab, sketchParentObject.transform);
                SketchController controller = sketchObj.GetComponent<SketchController>();
                controller.Sketch = sketch;
                currentSketches.Add(controller);
                await controller.SetImageFromUrl(sketch.ImageUrl);
            }
        }


        private void RemoveSketches()
        {
            foreach (var sketchCon in currentSketches)
            {
                Destroy(sketchCon.gameObject);
            }

            _sketches = new();
            currentSketches = new();
        }
    }
}