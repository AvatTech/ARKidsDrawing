using UnityEngine;

namespace AvatAdmobExtension.Script.ScriptableObject.AdmobData
{
    [CreateAssetMenu(menuName = "Admob Data", fileName = "Admob Data")]
    public class AdmobData : UnityEngine.ScriptableObject
    {
        [SerializeField] public string BannerUnitID;
        [SerializeField] public string InterstitialUnitID;
        [SerializeField] public string RewardedInterstitialUnitID;
        [SerializeField] public string NativeUnitID;
    }
}
