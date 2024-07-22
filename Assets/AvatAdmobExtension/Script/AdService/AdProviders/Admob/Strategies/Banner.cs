using System;
using System.Collections;
using System.Collections.Generic;
using AvatAdmobExtension.Script.Abstraction;
using GoogleMobileAds.Api;
using UnityEngine;

namespace AvatAdmobExtension.Script.AdService.AdProviders.Admob.Strategies
{
    public class Banner : AdBase, IAdStrategy
    {
        private BannerView _bannerView;

        private string _adUnitId;
        private AdSize _adSize;
        private AdPosition _adPosition;

        // Actions:
        public Action OnLoaded;
        public Action<LoadAdError> OnLoadFailed;
        public Action<AdValue> OnAdPaid;
        public Action OnImpressionRecorded;
        public Action OnAddClicked;
        public Action OnFullScreenOpened;
        public Action OnFullScreenClosed;


        public Banner(AdSize adSize, AdPosition adPosition)
        {
            _adSize = adSize;
            _adPosition = adPosition;
            // Set banner google ads id

            if (adManager == null)
                Debug.Log("ad manager is null");


            _adUnitId = adManager.admobData.BannerUnitID;
        }


        /// <summary>
        /// Creates a 320x50 banner view at top of the screen.
        /// </summary>
        private void CreateBannerView()
        {
            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
                DestroyBannerView();

            // Create a 320x50 banner at top of the screen
            _bannerView = new BannerView(_adUnitId, _adSize, _adPosition);


            SubscribeToEvents();
        }

        /// <summary>
        /// Creates the banner view and loads a banner ad.
        /// </summary>
        public bool LoadAd()
        {
            // create an instance of a banner view first.
            if (_bannerView == null)
                CreateBannerView();

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            _bannerView?.LoadAd(adRequest);

            _bannerView?.Hide();

            return false;
        }


        public bool ShowAd()
        {
            if (_bannerView == null)
                LoadAd();

            _bannerView?.Show();

            return true;
        }
        
        
        public IEnumerator LoadThenShow()
        {
            // create an instance of a banner view first.
            if (_bannerView == null)
                CreateBannerView();

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            _bannerView?.LoadAd(adRequest);

            if (_bannerView is not null)
            {
                _bannerView.OnBannerAdLoaded += () => { _bannerView.Show(); };
            }

            yield return null;
        }

        /// <summary>
        /// Destroys the banner view.
        /// </summary>
        private void DestroyBannerView()
        {
            if (_bannerView != null)
            {
                _bannerView.Destroy();
                _bannerView = null;
            }
        }

        /// <summary>
        /// listen to events the banner view may raise.
        /// </summary>
        private void SubscribeToEvents()
        {
            // Raised when an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += OnLoaded;

            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += OnLoadFailed;

            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += OnAdPaid;

            // Raised when an impression is recorded for an ad.
            _bannerView.OnAdImpressionRecorded += OnImpressionRecorded;

            // Raised when a click is recorded for an
            _bannerView.OnAdClicked += OnAddClicked;

            // Raised when an ad opened full screen content.
            _bannerView.OnAdFullScreenContentOpened += OnFullScreenOpened;

            // Raised when the ad closed full screen content.
            _bannerView.OnAdFullScreenContentClosed += OnFullScreenClosed;
        }
    }
}