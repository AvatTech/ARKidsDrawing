using System;
using System.Threading.Tasks;
using AvatAdmobExtension.Script.Manager;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;
using Utills;

namespace Firebase.RemoteConfig
{
    public class RemoteConfig
    {
        public readonly UnityEvent<bool, bool, bool> OnDataFetched = new();

        public RemoteConfig()
        {
            OnDataFetched.AddListener(DataFetched);
            
            FetchDataAsync();

            FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
                += ConfigUpdateListenerEventHandler;
        }

        public Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            var fetchTask =
                FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                    TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError(
                    $"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            // Fetch successful. Parameter values must be activated to use.
            remoteConfig.ActivateAsync()
                .ContinueWithOnMainThread(
                    task =>
                    {
                        var native = remoteConfig.GetValue(Constants.NativeAdShow).BooleanValue;
                        var banner = remoteConfig.GetValue(Constants.BannerAdShow).BooleanValue;
                        var interstitial = remoteConfig.GetValue(Constants.InterstitialAdShow).BooleanValue;

                        OnDataFetched.Invoke(native, banner, interstitial);

                        Debug.Log(
                            $"Remote data loaded and ready for use. native: {native}, banner:{banner}, interstitial:{interstitial}.");
                    });
        }

// Handle real-time Remote Config events.
        void ConfigUpdateListenerEventHandler(
            object sender, ConfigUpdateEventArgs args)
        {
            if (args.Error != RemoteConfigError.None)
            {
                Debug.Log(String.Format("Error occurred while listening: {0}", args.Error));
                return;
            }

            Debug.Log("Updated keys: " + string.Join(", ", args.UpdatedKeys));
            // Activate all fetched values and then display a welcome message.
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            remoteConfig.ActivateAsync().ContinueWithOnMainThread(
                task =>
                {
                    Debug.Log("remote config changed!");

                    var native = remoteConfig.GetValue(Constants.NativeAdShow).BooleanValue;
                    var banner = remoteConfig.GetValue(Constants.BannerAdShow).BooleanValue;
                    var interstitial = remoteConfig.GetValue(Constants.InterstitialAdShow).BooleanValue;

                    Debug.Log(
                        $"Remote data loaded and ready for use. native: {native}, banner:{banner}, interstitial:{interstitial}.");

                    OnDataFetched.Invoke(native, banner, interstitial);
                });
        }

        private void DataFetched(bool native, bool banner, bool interstitial)
        {
            AdManager.Instance.CanNativeAdShow = native;
            AdManager.Instance.CanBannerAdShow = banner;
            AdManager.Instance.CanInterstitialAdShow = interstitial;
        }
    }
}