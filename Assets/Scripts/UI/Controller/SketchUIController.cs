using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvatAdmobExtension.Script.Manager;
using Categories.Utills;
using Extensions.Unity.ImageLoader;
using NSubstitute.Extensions;
using Repositories;
using Sketches.Controller;
using Sketches.Model;
using Sketches.Utills;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utills;
using Zenject;

namespace UI.Controller
{
    public class SketchUIController : MonoBehaviour
    {
        [Inject] private readonly IAPRepository _iapRepository;

        [SerializeField] private GameObject sketchPrefab;
        [SerializeField] private GameObject sketchParentObject;
        [SerializeField] private RawImage categoryIcon;
        [SerializeField] private TextMeshProUGUI categoryTitleText;

        [Space, SerializeField] private GameObject purchasePanel;

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

            if (PlayerPrefs.HasKey(Constants.KeyIsComingFromAR))
            {
                AdManager.Instance.InterstitialAd.LoadAd();
                StartCoroutine(StartShowingAd());
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

        private IEnumerator StartShowingAd()
        {
            yield return new WaitUntil(() => AdManager.Instance.InterstitialAd.IsAdReady());

            AdManager.Instance.InterstitialAd.ShowAd();
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

                if (sketch.IsPremium && !_iapRepository.IsPurchased())
                {
                    controller.ConfigurePremium();

                    controller.AddOnSketchClickedListener(() =>
                    {
                        // iap purchase opens
                        purchasePanel.SetActive(true);
                    });
                }
                else
                {
                    CurrentSketchHolder.Instance.CurrentSketchUrl = sketch.ImageUrl;
                    controller.AddOnSketchClickedListener(() => { SceneManager.LoadScene("AppScene"); });
                }

                _currentSketchesObjects.Add(controller);
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