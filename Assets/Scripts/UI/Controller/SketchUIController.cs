using System.Collections.Generic;
using System.Threading.Tasks;
using Categories.Utills;
using Extensions.Unity.ImageLoader;
using NSubstitute.Extensions;
using Sketches.Controller;
using Sketches.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class SketchUIController : MonoBehaviour
    {
        [SerializeField] private GameObject sketchPrefab;
        [SerializeField] private GameObject sketchParentObject;
        [SerializeField] private RawImage categoryIcon;
        [SerializeField] private TextMeshProUGUI categoryTitleText;

        private readonly List<SketchController> _currentSketchesObjects = new();

        private List<Sketch> _sketches = new();


        private async void OnEnable()
        {
            if (CurrentCategoryManager.Instance.CurrentCategory == null)
            {
                categoryIcon.texture = null;
                categoryTitleText.text = string.Empty;
                return;
            }

            await ImageLoader.LoadSprite(CurrentCategoryManager.Instance.CurrentCategory.CoverImageUrl)
                .ThenSet(categoryIcon);
            categoryTitleText.text = CurrentCategoryManager.Instance.CurrentCategory.Name;

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
            await InstantiateSketches(_sketches);
        }


        private async Task InstantiateSketches(List<Sketch> sketches)
        {

            var index = 1;
            foreach (var sketch in sketches)
            {
                var sketchObj = Instantiate(sketchPrefab, sketchParentObject.transform);
                sketchObj.name = $"{index++}";
                var controller = sketchObj.GetComponent<SketchController>();
                controller.Sketch = sketch;
                _currentSketchesObjects.Add(controller);
                controller.ConfigurePremium();
                await controller.SetImageFromUrl(sketch.ImageUrl);
            }
        }


        private void RemoveSketches()
        {
            for (var i = _currentSketchesObjects.Count - 1; i >= 0; i--)
            {
                Destroy(_currentSketchesObjects[i].gameObject);
            }

            _sketches.Clear();
            _currentSketchesObjects.Clear();
        }
    }
}