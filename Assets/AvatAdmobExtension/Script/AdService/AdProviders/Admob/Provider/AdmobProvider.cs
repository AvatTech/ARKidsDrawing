using AvatAdmobExtension.Script.AdService.Abstraction;
using GoogleMobileAds.Api;
using UnityEngine;

namespace AvatAdmobExtension.Script.AdService.AdProviders.Admob.Provider
{
    public class AdmobProvider : IAdProvider
    {
        public void Initialize()
        {
            MobileAds.Initialize(status => { Debug.Log($"Mobile Ads status: {status}"); });
        }
    }
}