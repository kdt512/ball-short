using System;
using UnityEngine;

public static class AhaTrackingManager {

    public static void Tracking(string action, string param1 = null, string param2 = null) {
        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("tracking", action, param1, param2);
    }

    public static void ShowPrivacyAgreement()
    {
        using var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // get activity
        var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");

        using var ahaSDKClass = new AndroidJavaClass("com.tiger.games.AhaSDK");
        ahaSDKClass.CallStatic("showPrivacyAgreement", activity);
    }
}

public class TrackingEvent {
    public static string appStart = "app_start";
    public static string loadingBegin = "loading_begin";
    public static string loadingEnd = "loadingEnd";
    public static string gameStart = "game_start";
    public static string tutorialBegin = "tutorial_begin";
    public static string tutorialEnd = "tutorial_end";
    public static string levelBegin = "level_begin";
    public static string levelEnd = "level_end";
    public static string levelReward = "level_reward";
}