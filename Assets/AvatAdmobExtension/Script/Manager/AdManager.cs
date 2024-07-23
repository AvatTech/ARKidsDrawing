using AvatAdmobExtension.Script.Abstraction;
using AvatAdmobExtension.Script.AdService.Abstraction;
using AvatAdmobExtension.Script.AdService.AdProviders.Admob.Provider;
using AvatAdmobExtension.Script.AdService.AdProviders.Admob.Strategies;
using AvatAdmobExtension.Script.ScriptableObject.AdmobData;
using GoogleMobileAds.Api;
using UnityEngine;

namespace AvatAdmobExtension.Script.Manager
{
    public class AdManager : MonoBehaviour
    {
        public AdmobData admobData;


        [Space, SerializeField] private bool admob = true;
        [SerializeField] private bool unityAds = false;

        [Space, SerializeField] private bool Banner;
        [SerializeField] private bool Interstitial;
        [SerializeField] private bool Rewarded;

        [SerializeField] private AdPosition adPosition;


        private static AdManager _instance;

        public Banner BannerAd;
        public Interstitial InterstitialAd;
        public IAdStrategy RewardedAd;
        public NativeAdHandler NativeAdHandler;


        public bool CanNativeAdShow { get; set; }
        public bool CanBannerAdShow { get; set; }
        public bool CanInterstitialAdShow { get; set; }


        public static AdManager Instance =>
            _instance ?? (_instance = new GameObject("AdManager").AddComponent<AdManager>());

        private IAdProvider _iAdProvider = new AdmobProvider();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }

            GetAdmobConfigFile();
            Initialize();
        }


        private void GetAdmobConfigFile()
        {
            admobData = Resources.Load<AdmobData>("AdmobData");
            if (admobData)
                Debug.Log("Admob data is not null");
        }


        // initialize admob
        private void Initialize()
        {
            if (admob)
                _iAdProvider.Initialize();

            if (Banner)
                BannerAd = new Banner(AdSize.Banner, adPosition);

            if (Interstitial)
                InterstitialAd = new Interstitial();

            if (Rewarded)
                RewardedAd = new Rewarded((reward => { }));

            BannerAd.LoadAd();
            InterstitialAd.LoadAd();
            RewardedAd.LoadAd();
        }
    }
}