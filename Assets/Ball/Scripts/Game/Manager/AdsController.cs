using System;
using UnityEngine;
using UnityEngine.Events;

public class AdsController : Singleton<AdsController>
{
    #region Field
    public bool pauseInGame;
    public bool isNoAds;
    private bool isHideBanner;
    // minimum seconds between 2 show interstitial
    private int interstitialInterval = 30;
    private DateTime lastShowInterstialTime = new DateTime(0);

    #endregion
    #region Propertives
    public bool HasJustShowAds
    {
        get;
        set;
    }

    public bool IsHideBanner { get { return isHideBanner; } }
    /// <summary>
    /// seconds between 2 show interstitial
    /// </summary>
    public int InterstitialInterval
    {
        get { return interstitialInterval; }
        set { interstitialInterval = value; }
    }

    #endregion
    /// <summary>
    /// Show rewared ads
    /// </summary>
    /// <param name="placement">where ads placed</param>
    /// <param name="completeAction">action when user have watched ads completely. Add reward to user</param>
    /// <param name="callbackAction">action when ads finish show (user watched or rejected)</param>
    public void ShowRewardedVideoAd(string placement,
        UnityAction<bool> completeAction = null, UnityAction callbackAction = null)
    {
#if UNITY_EDITOR
        completeAction?.Invoke(true);
        callbackAction?.Invoke();
        return;
#elif UNITY_WEBGL
        Bridge.Instance.ShowFbRewardVideoAds(placement, completeAction, callbackAction);
        return;
#endif
        //if (Commons.IsConnectionNetwork())
        //{
        //    IronSourceManager.Instance?.ShowRewardedVideoAd(placement, completeAction, callbackAction);
        //}
        //else
        //{
        //    Commons.ShowDialog("NOTIFY", "Cannot connect to network. Try again latter!",
        //        () => { callbackAction?.Invoke(); });
        //}

    }

    /// <summary>
    /// Show Interstitial ads (ads between different screen)
    /// </summary>
    /// <param name="placement"> ads placement where ads place</param>
    /// <param name="callbackAction">action after show ads</param>
    public void ShowInterstitialAd(string placement, UnityAction callbackAction = null)
    {
        Debug.Log("AdManager - ShowInterstitialAd");
        if (isNoAds)
        {
            callbackAction?.Invoke();
            return;
        }

        if ((DateTime.Now - lastShowInterstialTime).TotalSeconds < interstitialInterval)
        {
            Debug.LogWarning($"AdManager - Do not show Ads in {interstitialInterval}s");
            return;
        }
        lastShowInterstialTime = DateTime.Now;
#if UNITY_EDITOR
        callbackAction?.Invoke();
        return;
#elif UNITY_WEBGL
        Bridge.Instance.ShowFbInterstitalAds(placement, callbackAction);
        return;
#endif
        //if (Commons.IsConnectionNetwork())
        //{
        //    IronSourceManager.Instance?.ShowInterstitialAd(placement, callbackAction);
        //}
        //else
        //{
        //    Debug.LogWarning("AdManager - ShowInterstitialAd IsConnectionNetwork false");
        //    callbackAction?.Invoke();
        //}
    }

    /// <summary>
    /// Show App Open Ads that show when app open after paused for long time, 
    /// or change from background
    /// </summary>
    public void ShowAppOpenAd()
    {
        //OpenAdManager.Instance?.ShowAd();
    }
    /// <summary>
    /// Show banner if it had been loaded
    /// </summary>
    public void ShowBanner()
    {
        isHideBanner = false;
#if UNITY_WEBGL
        Bridge.Instance.ShowFbBanner();
#else
            IronSourceManager.Instance?.ShowBannerAd();
#endif
    }
    /// <summary>
    /// Hide banner that had been loaded, 
    /// else do not show banner when it will be loaded
    /// </summary>
    public void HideBanner()
    {
        isHideBanner = true;
#if UNITY_WEBGL
        Bridge.Instance.HideFbBanner();
#else
            IronSourceManager.Instance?.HideBannerAd();
#endif
    }

    public void ShowRewardedInterstitialAd(string placement,
        UnityAction<bool> completeAction = null, UnityAction callbackAction = null)
    {
#if UNITY_EDITOR
        completeAction?.Invoke(true);
        callbackAction?.Invoke();
        return;
#elif UNITY_WEBGL
        Bridge.Instance.ShowFbRewardInterstitalAds(placement, completeAction, callbackAction);
        return;
#endif
        //if (Commons.IsConnectionNetwork())
        //{
        //    IronSourceManager.Instance?.ShowRewardedVideoAd(placement, completeAction, callbackAction);
        //}
        //else
        //{
        //    Commons.ShowDialog("NOTIFY", "Cannot connect to network. Try again latter!",
        //        () => { callbackAction?.Invoke(); });
        //}
    }
}
