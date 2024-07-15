using System;
using System.Collections;
using DG.Tweening;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

public class AppOpenAdManager : Singleton<AppOpenAdManager>
{
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/9257395921";
#elif UNITY_IPHONE
    string _adUnitId = "ca-app-pub-3940256099942544/5575463023";
#else
    private string _adUnitId = "unused";
#endif

    private AppOpenAd appOpenAd;
    private DateTime _expireTime;
    private bool isShowAdsState = false;
    public bool IsAdAvailable => appOpenAd != null && DateTime.Now < _expireTime;
    public bool acceptAds => timerCountdownAds <= 0;
    public float timerCountdownAds;

    protected override void Awake()
    {
        base.Awake();

        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    IEnumerator AceeptAdsInit()
    {
        while (timerCountdownAds > 0)
        {
            yield return null;
            timerCountdownAds -= Time.deltaTime;
        }
    }

    private void OnAppStateChanged(AppState state)
    {
        
    }

    private void OnApplicationPause(bool pause)
    {
        if (IsAdAvailable && !isShowAdsState)
        {
            if (!pause && acceptAds)
            {
                DOVirtual.DelayedCall(.4f, () =>
                {
                    if (AdsController.Instance.hasAdsOpen)
                    {
                        AdsController.Instance.hasAdsOpen = false;
                    }
                    else
                    {
                        ShowAppOpenAd();
                        timerCountdownAds = 60;
                        StartCoroutine(AceeptAdsInit());
                    }
                });
            }
        }
    }

    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd(Action callback = null)
    {
        if (!AdsController.isInitialized.HasValue)
        {
            Debug.Log("[AppOpenAds] Admod is not init");
            return;
        }

        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            AdsController.Instance.banner.ShowAd();
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_adUnitId, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                // App open ads can be preloaded for up to 4 hours.
                _expireTime = DateTime.Now + TimeSpan.FromHours(4);

                appOpenAd = ad;
                RegisterEventHandlers(ad);

                callback?.Invoke();

                FirebaseManager.Instance.LogEventAds(AdUnitType.Appopen, AdEventType.Load);
            });
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("App open ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));

            DataManager.TotalAppOpenAdsValue += adValue.Value;
            FirebaseManager.Instance.LogEventAds(AdUnitType.Appopen, DataManager.TotalAppOpenAdsValue);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
            FirebaseManager.Instance.LogEventAds(AdUnitType.Appopen, AdEventType.Impression);
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => { Debug.Log("App open ad was clicked."); };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => { Debug.Log("App open ad full screen content opened."); };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
    }

    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            AdsController.Instance.banner.HideAd();
            appOpenAd.Show();
            FirebaseManager.Instance.LogEventAds(AdUnitType.Appopen, AdEventType.Show);
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }

    public void ShowOpenAds()
    {
        if (IsAdAvailable)
        {
            // khong show open ngay ban dau game
            //ShowAppOpenAd();
            timerCountdownAds = 60;
            StartCoroutine(AceeptAdsInit());
        }
    }
}