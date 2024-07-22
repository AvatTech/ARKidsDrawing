using System;
using System.Collections;
using System.Collections.Generic;
using AvatAdmobExtension.Script.Abstraction;
using GoogleMobileAds.Api;
using UnityEngine;

namespace AvatAdmobExtension.Script.AdService.AdProviders.Admob.Strategies
{
    public class Interstitial : AdBase, IAdStrategy
    {
        private string _adUnitId;

        private InterstitialAd _interstitialAd;


        // Actions:
        public Action<AdValue> OnAdPaid;
        public Action OnImpressionRecorded;
        public Action OnAddClicked;
        public Action OnFullScreenOpened;
        public Action OnFullScreenClosed;
        public Action<AdError> OnFullScreenFailed;

        public Interstitial()
        {
            _adUnitId = adManager.admobData.InterstitialUnitID;
        }

        /// <summary>
        /// Loads the interstitial ad.
        /// </summary>
        public bool LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            bool adStatus = false;

            // send the request to load the ad.
            InterstitialAd.Load(_adUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.Log("interstitial ad failed to load an ad " +
                                  "with error : " + error);
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());


                    _interstitialAd = ad;
                    adStatus = true;
                });

            SubscribeToEvents();


            return adStatus;
        }


        /// <summary>
        /// Shows the interstitial ad.
        /// </summary>
        public bool ShowAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                _interstitialAd.Show();
                return true;
            }
            else
            {
                Debug.Log("Interstitial ad is not ready yet.");
                return false;
            }
        }


        public IEnumerator LoadThenShowCoroutine()
        {
            LoadAd();

            yield return new WaitWhile(() => _interstitialAd == null);
        }

        public bool IsAdReady()
        {
            return _interstitialAd != null;
        }


        /// <summary>
        /// listen to events the banner view may raise.
        /// </summary>
        private void SubscribeToEvents()
        {
            // Raised when the ad is estimated to have earned money.
            _interstitialAd.OnAdPaid += OnAdPaid;

            // Raised when an impression is recorded for an ad.
            _interstitialAd.OnAdImpressionRecorded += OnImpressionRecorded;

            // Raised when a click is recorded for an ad.
            _interstitialAd.OnAdClicked += OnAddClicked;

            // Raised when an ad opened full screen content.
            _interstitialAd.OnAdFullScreenContentOpened += OnFullScreenOpened;

            // Raised when the ad closed full screen content.
            _interstitialAd.OnAdFullScreenContentClosed += OnFullScreenClosed;

            // Raised when the ad failed in full screen content
            _interstitialAd.OnAdFullScreenContentFailed += OnFullScreenFailed;
        }
    }
}