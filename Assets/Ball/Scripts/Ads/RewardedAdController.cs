using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class RewardedAdController
{
#if UNITY_ANDROID
        private const string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private const string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;
    private Action<bool> showCompleteCB;
    private string responseId;

    /// Loads the ad.
    public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading rewarded ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                Utils.Invoke(AdsController.Instance, LoadAd, 3);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                Utils.Invoke(AdsController.Instance, LoadAd, 3);
                return;
            }

            // The operation completed successfully.
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
            _rewardedAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);
            responseId = _rewardedAd.GetResponseInfo().GetResponseId();
        });
    }

    /// Shows the ad.
    public void ShowAd(Action<bool> callback = null)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            showCompleteCB = callback;
            Debug.Log("Showing rewarded ad.");
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}", reward.Amount, reward.Type));
            });
            FirebaseManager.Instance.LogEventAds(AdUnitType.Rewarded, responseId == _rewardedAd.GetResponseInfo().GetResponseId());
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
            var notiPopup = UIManager.Instance.OpenUI<NotiPopup>(DialogType.POPUP_NOTI);
            notiPopup.ShowAsInfo("NOTIFY!", "Have no ads to show! Please wait for loading ads and try again");
        }

    }

    /// Destroys the ad.
    public void DestroyAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

    }

    /// Logs the ResponseInfo.
    public void LogResponseInfo()
    {
        if (_rewardedAd != null)
        {
            var responseInfo = _rewardedAd.GetResponseInfo();
            UnityEngine.Debug.Log(responseInfo);
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
            DataManager.TotalRewardAdsValue += adValue.Value;
            FirebaseManager.Instance.LogEventAds(AdUnitType.Rewarded, DataManager.TotalRewardAdsValue);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
            FirebaseManager.Instance.LogEventAds(AdUnitType.Rewarded, AdEventType.Impression);
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            showCompleteCB?.Invoke(true);
            Utils.Invoke(AdsController.Instance, LoadAd, 3);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : "
                + error);
            showCompleteCB?.Invoke(false);
            Utils.Invoke(AdsController.Instance, LoadAd, 3);
        };
    }
}
