using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Model;
using Categories.Services;
using Categories.Utills;
using Sketches.Controller;
using Sketches.Model;
using Sketches.Services;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace UI.Controller
{
    public class SketchUIController : MonoBehaviour
    {
        [SerializeField] private GameObject sketchPrefab;
        [SerializeField] private GameObject sketchParentObject;

        private List<SketchController> currentSketchesObjects = new();

        private List<Sketch> _sketches = new();


        private async void OnEnable()
        {
            if (CurrentCategoryManager.Instance.CurrentCategory == null)
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
            //_sketches = await _fetchSketchesService.FetchSketchesById(_currentCategoryManager.CurrentCategory.Id);

            _sketches = new List<Sketch>(CurrentCategoryManager.Instance.CurrentCategory.Sketches);
            await SetUpSketchItems(_sketches);
        }


        private async Task SetUpSketchItems(List<Sketch> sketches)
        {
            
            Debug.Log("fetching sketches...");
            
            var index = 1;
            foreach (var sketch in sketches)
            {
                var sketchObj = Instantiate(sketchPrefab, sketchParentObject.transform);
                sketchObj.name = $"{index++}";
                var controller = sketchObj.GetComponent<SketchController>();
                controller.Sketch = sketch;
                currentSketchesObjects.Add(controller);
                await controller.SetImageFromUrl(sketch.ImageUrl);
            }
        }


        private void RemoveSketches()
        {
            for (var i = currentSketchesObjects.Count - 1; i >= 0; i--)
            {
                Destroy(currentSketchesObjects[i].gameObject);
            }

            _sketches.Clear();
            currentSketchesObjects.Clear();
        }
    }
}