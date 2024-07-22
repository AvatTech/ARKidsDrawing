using System;
using AvatAdmobExtension.Script.Abstraction;
using GoogleMobileAds.Api;
using UnityEngine;

namespace AvatAdmobExtension.Script.AdService.AdProviders.Admob.Strategies
{
    public class Rewarded : AdBase, IAdStrategy
    {
        private string _adUnitId;
        private RewardedAd _rewardedAd;

        public Action<Reward> OnRewardSuccess;


        // Actions:
        public Action<AdValue> OnAdPaid;
        public Action OnImpressionRecorded;
        public Action OnAddClicked;
        public Action OnFullScreenOpened;
        public Action OnFullScreenClosed;
        public Action<AdError> OnFullScreenFailed;

        public Rewarded(Action<Reward> onRewardSuccess)
        {
            OnRewardSuccess = onRewardSuccess;
            _adUnitId = adManager.admobData.RewardedInterstitialUnitID;
        }

        /// <summary>
        /// Loads the rewarded ad.
        /// </summary>
        public bool LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            bool isOk = false;

            // send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        isOk = false;
                        return;
                    }


                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                    isOk = true;
                });

            SubscribeToEvents();

            return isOk;
        }


        public bool ShowAd()
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (_rewardedAd == null)
                LoadAd();


            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) =>
                {
                    OnRewardSuccess.Invoke(reward);
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }

            return true;
        }


        /// <summary>
        /// listen to events the banner view may raise.
        /// </summary>
        private void SubscribeToEvents()
        {
            // Raised when the ad is estimated to have earned money.
            _rewardedAd.OnAdPaid += OnAdPaid;

            // Raised when an impression is recorded for an ad.
            _rewardedAd.OnAdImpressionRecorded += OnImpressionRecorded;

            // Raised when a click is recorded for an ad.
            _rewardedAd.OnAdClicked += OnAddClicked;

            // Raised when an ad opened full screen content.
            _rewardedAd.OnAdFullScreenContentOpened += OnFullScreenOpened;

            // Raised when the ad closed full screen content.
            _rewardedAd.OnAdFullScreenContentClosed += OnFullScreenClosed;

            // Raised when the ad failed in full screen content
            _rewardedAd.OnAdFullScreenContentFailed += OnFullScreenFailed;
        }
    }
}