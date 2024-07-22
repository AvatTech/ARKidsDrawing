using System;
using System.Collections;
using AvatAdmobExtension.Script.Abstraction;
using AvatAdmobExtension.Script.Manager;
using GoogleMobileAds.Api;
using GoogleMobileAds.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvatAdmobExtension.Script.AdService.AdProviders.Admob.Strategies
{
    public class NativeAdHandler : MonoBehaviour, IAdStrategy
    {
        [SerializeField] private GameObject headlineObject;
        [SerializeField] private GameObject descriptionObject;
        [SerializeField] private GameObject iconObject;

        private string _adUnitID;
        private AdManager _adManager;
        private NativeAd _nativeAd;
        private bool _nativeAdLoaded;

        private TextMeshProUGUI _headlineText;
        private TextMeshProUGUI _descriptionText;
        private RawImage _iconImage;

        private void Awake()
        {
            if (_adManager == null)
                Debug.Log("ad manager is null");

            _adUnitID = _adManager.admobData.BannerUnitID;
        }

        private void Start()
        {
            // init
            _headlineText = headlineObject.GetComponent<TextMeshProUGUI>();
            _descriptionText = descriptionObject.GetComponent<TextMeshProUGUI>();
            _iconImage = iconObject.GetComponent<RawImage>();

            MobileAds.Initialize(status =>
            {
                Debug.Log("Native Ad init finished!");
                StartCoroutine(StartShowingNativeAd());
            });
        }
        
        private void RequestNativeAd()
        {
            var adLoader = new AdLoader.Builder(_adUnitID)
                .ForNativeAd()
                .Build();
            
            adLoader.OnNativeAdLoaded += HandleNativeAdLoaded;
            adLoader.OnAdFailedToLoad += HandleNativeAdFailedToLoad;

            var req = new AdRequest();
            adLoader.LoadAd(req);
        }
        
        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
        {
            Debug.Log("Native ad loaded.");
            _nativeAd = args.nativeAd;
            _nativeAdLoaded = true;
        }

        private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
        }
        
        public bool LoadAd()
        {
            return true;
        }

        public bool ShowAd()
        {
            // Get string of the headline asset.
            _headlineText.text = _nativeAd.GetHeadlineText();
            _descriptionText.text = _nativeAd.GetBodyText();
            _iconImage.texture = _nativeAd.GetIconTexture();

            // Adding box colliders
            headlineObject.AddComponent<BoxCollider2D>();
            descriptionObject.AddComponent<BoxCollider2D>();
            iconObject.AddComponent<BoxCollider2D>();

            if (!_nativeAd.RegisterHeadlineTextGameObject(headlineObject))
            {
                Debug.Log("Register headline failed");
            }

            if (!_nativeAd.RegisterBodyTextGameObject(descriptionObject))
            {
                Debug.Log("Register decs failed");
            }

            if (!_nativeAd.RegisterHeadlineTextGameObject(iconObject))
            {
                Debug.Log("Register icon failed");
            }

            return true;
        }
        
        private IEnumerator StartShowingNativeAd()
        {
            while (true)
            {
                RequestNativeAd();

                //LOADING AD LOGIC
                iconObject.gameObject.SetActive(false);
                descriptionObject.gameObject.SetActive(false);
                _headlineText.text = "Loading...";

                // Wait till add loads
                yield return new WaitUntil(() => _nativeAdLoaded);

                // Ad loaded logic
                ShowAd();
                iconObject.gameObject.SetActive(true);
                descriptionObject.gameObject.SetActive(true);
            }
        }
    }
}