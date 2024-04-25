using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

namespace Firebase
{
    public class FirebaseInit : MonoBehaviour
    {
        private FirebaseApp _app;

        private void Start()
        {
            Logger.Instance.InfoLog("Start Checking Firebase...");
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    Logger.Instance.InfoLog("Checking Firebase...");
                    _app = FirebaseApp.DefaultInstance;
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                    Crashlytics.Crashlytics.ReportUncaughtExceptionsAsFatal = true;

                    Logger.Instance.InfoLog("Firebase is ok");

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    Logger.Instance.InfoLog($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });


            Logger.Instance.InfoLog("End Checking Firebase...");
        }
    }
}