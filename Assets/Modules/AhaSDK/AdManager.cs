using System;
using UnityEngine;
using Unity.VisualScripting;

public class AdManager : Singleton<AdManager>
{
    private static Action<bool> onRewardedAction;

    void Start()
    {
        Initialize();
    }

    public static void Initialize()
    {
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // get activity
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");

        // get application
        var application = activity.Call<AndroidJavaObject>("getApplication");

        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("initialize", application);
    }

    public void OnInitializationCompleted(string message)
    {
        Debug.Log("OnInitializationCompleted: " + message);
        LoadInterstitialAd();
        LoadRewardedAd();
        ShowBanner();
    }

    public static void LoadRewardedAd()
    {
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // get activity
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");

        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("loadReward", activity);
    }

    public static void ShowFloatAd()
    {
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // get activity
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");

        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("showFloatAds", activity);
    }

    public static void ShowRewardedVideoAd(Action<bool> action, Action callbackAction = null)
    {
        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");

        var isRewardedAdLoaded = ahaSDKClass.CallStatic<bool>("isRewardedAdLoaded");
        if (!isRewardedAdLoaded)
        {
            LoadRewardedAd();
            //if (Commons.IsConnectionNetwork())
            //{
            //    Commons.ShowDialog("NOTIFY", "No ad to show. Try again latter!",
            //        () => { callbackAction?.Invoke(); });
            //}
            //else
            //{
            //    Commons.ShowDialog("NOTIFY", "Cannot connect to network. Try again latter!",
            //        () => { callbackAction?.Invoke(); });
            //}
            return;
        }
        onRewardedAction = action;
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // get activity
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");
        ahaSDKClass.CallStatic("showReward", activity);
    }

    public static void LoadInterstitialAd()
    {
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");
        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("loadInterstitial", activity);
    }

    public static void ShowInterstitialAd()
    {
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");
        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("showInterstitial", activity);
    }

    public static void ShowBanner()
    {
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");
        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("showBottomBanner", activity);
    }

    public static bool IsRewardedAdLoaded()
    {
        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        var isLoaded = ahaSDKClass.CallStatic<bool>("isRewardedAdLoaded");
        return isLoaded;
    }

    public void OnRewarded(string message)
    {
        onRewardedAction?.Invoke(message == "rewarded");
        LoadRewardedAd();
    }
}