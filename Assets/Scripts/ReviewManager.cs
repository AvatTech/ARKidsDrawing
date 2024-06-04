using Bayegan.Builder;
using Bayegan.Storage.Abstractions;
using UnityEngine;

public class ReviewManager
{
    private const string ARSessionKey = "ARSession";
    private const string LaunchCounterKey = "LaunchCounter";

    private readonly IBayeganDictionary _bayegan = new BayeganDictionaryBuilder().Build();

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
    
    public void Review()
    {
        var launcherCounter = _bayegan.Load(LaunchCounterKey, 0);
        var arSession = _bayegan.Load(ARSessionKey, false);
        Debug.Log($"launcher counter: {launcherCounter}, ar session: {arSession}");
    }
}