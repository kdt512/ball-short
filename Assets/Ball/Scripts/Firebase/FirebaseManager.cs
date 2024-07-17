using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
using System;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool firebaseInitialized = false;

    public virtual void Start()
    {
        if (string.IsNullOrEmpty(DataManager.UserId))
            DataManager.UserId = SystemInfo.deviceUniqueIdentifier;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
                InitializeFirebase();
            else
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        });
    }

    void InitializeFirebase()
    {
        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Debug.Log("Set user properties.");
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
        // Set the user ID.
        FirebaseAnalytics.SetUserId("uber_user_510");
        // Set default session duration values.
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;

        FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
        Crashlytics.ReportUncaughtExceptionsAsFatal = true;
    }

    // Update is called once per frame
    public void LogEventCompleteTutorial()
    {
        LogEvent(FirebaseAnalytics.EventTutorialComplete, new Parameter(FirebaseAnalytics.ParameterCharacter, SystemInfo.deviceUniqueIdentifier));
    }

    public void LogEventLevelUp(int level)
    {
        LogEvent(FirebaseAnalytics.EventLevelUp, new Parameter("level", level));
    }

    public void LogEvent(string keyEvent, Parameter parameter)
    {
        Debug.Log("LogEvent: "+keyEvent+"  "+ firebaseInitialized);
        if (!firebaseInitialized) return;

        Parameter[] adParams = new Parameter[]
        {
            parameter
        };
        FirebaseAnalytics.LogEvent(keyEvent, adParams);
    }

    public void LogEvent(string keyEvent, Parameter[] adParams)
    {
        if (!firebaseInitialized) return;
        FirebaseAnalytics.LogEvent(keyEvent, adParams);
    }


    public void LogEvent(string keyEvent, string keyData, string data)
    {
        if (!firebaseInitialized) return;

        Parameter[] adParams = new Parameter[]
        {
            new Parameter(keyData, data),
        };
        FirebaseAnalytics.LogEvent(keyEvent, adParams);
    }

    public void LogEventAds(AdUnitType adUnitType, AdEventType adEventType, string placement = "", string errorMsg = "",
        string addInfo = "")
    {
        var eventName = "Ads_" + adUnitType + "_" + adEventType;
        Parameter[] adParams = new Parameter[]
        {
            new Parameter("Placement", placement),
            new Parameter("AdInfo", (addInfo.Length > 90) ? addInfo.Substring(0, 90) : addInfo),
            new Parameter("ErrorMsg", errorMsg)
        };
        LogEvent(eventName, adParams);
    }

    public void LogEventAds(AdUnitType adUnitType, bool isMatched)
    {
        LogEvent("Ads_" + adUnitType, new Parameter("AdInfo", adUnitType + "_" + (isMatched ? "matched" : "not_matched")));
    }

    public void LogEventAds(AdUnitType adUnitType, long totalValue)
    {
        LogEvent("Ads_" + adUnitType, new Parameter("AdValue", totalValue));
    }
}

