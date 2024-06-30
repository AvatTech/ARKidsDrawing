using Bayegan.Builder;
using Bayegan.Storage.Abstractions;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class ReviewManager
{
    private const string ARSessionKey = "ARSession";
    private const string LaunchCounterKey = "LaunchCounter";

    private readonly IBayeganDictionary _bayegan = new BayeganDictionaryBuilder().Build();

    private bool _isReviewRequested;

    public ReviewManager()
    {
        IncreaseLauncherCounter();
    }

    public void EnableARSessionCheck()
    {
        _bayegan.Store(ARSessionKey, true);
    }

    public void IncreaseLauncherCounter()
    {
        var launcherCounter = _bayegan.Load(LaunchCounterKey, 0);
        _bayegan.Store(LaunchCounterKey, ++launcherCounter);
    }

    public void CheckReview()
    {
        var launcherCounter = _bayegan.Load(LaunchCounterKey, 0);
        var arSession = _bayegan.Load(ARSessionKey, false);

        if (arSession && launcherCounter % 2 == 0 && _isReviewRequested == false)
        {
            _isReviewRequested = true;
            Review();
        }
    }

    public void Review()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}