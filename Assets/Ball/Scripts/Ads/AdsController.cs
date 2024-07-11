
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdsController : Singleton<AdsController>
{
    #region AdsController

#if UNITY_ANDROID
    private const string banner_id = "ca-app-pub-3940256099942544/6300978111";
    private const string interstitial_id = "ca-app-pub-3940256099942544/1033173712";
    private const string reward_id = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private const string banner_id = "ca-app-pub-3940256099942544/2934735716";
    private const string interstitial_id = "ca-app-pub-3940256099942544/4411468910";
    private const string reward_id = "ca-app-pub-3940256099942544/1712485313";
#else
    private const string banner_id = "unused";
    private const string interstitial_id = "unused";
    private const string reward_id = "unused";
#endif

    internal static List<string> TestDeviceIds = new List<string>()
    {
        AdRequest.TestDeviceSimulator,
#if UNITY_IPHONE
        "96e23e80653bb28980d3f40beb58915c",
#elif UNITY_ANDROID
        "702815ACFC14FF222DA1DC767672A573"
#endif
    };
    private static bool? _isInitialized;
    private GoogleMobileAdsConsentController _consentController = new GoogleMobileAdsConsentController();

    private void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        MobileAds.SetRequestConfiguration(new RequestConfiguration
        {
            TestDeviceIds = TestDeviceIds
        });

        // If we can request ads, we should initialize the Google Mobile Ads Unity plugin.
        if (_consentController.CanRequestAds)
        {
            InitializeGoogleMobileAds();
        }

        // Ensures that privacy and consent information is up to date.
        InitializeGoogleMobileAdsConsent();
    }

    private void InitializeGoogleMobileAds()
    {
        if (_isInitialized.HasValue)
        {
            return;
        }

        _isInitialized = false;
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                _isInitialized = null;
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            _isInitialized = true;
        });
    }

    private void InitializeGoogleMobileAdsConsent()
    {
        _consentController.GatherConsent((string error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to gather consent with error: " + error);
            }
            else
            {
                Debug.Log("Google Mobile Ads consent updated: " + ConsentInformation.ConsentStatus);
            }

            if (_consentController.CanRequestAds)
            {
                InitializeGoogleMobileAds();
            }
        });
    }

    public void OpenAdInspector()
    {
        Debug.Log("Opening ad Inspector.");
        MobileAds.OpenAdInspector((AdInspectorError error) =>
        {
            if (error != null)
            {
                Debug.Log("Ad Inspector failed to open with error: " + error);
                return;
            }
            Debug.Log("Ad Inspector opened successfully.");
        });
    }

    public void OpenPrivacyOptions()
    {
        _consentController.ShowPrivacyOptionsForm((string error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to show consent privacy form with error: " + error);
            }
            else
            {
                Debug.Log("Privacy form opened successfully.");
            }
        });
    }
    #endregion

    #region Banner
    public BannerViewController banner = new BannerViewController();
    #endregion

    #region Interstitia
    public InterstitialAdController interstitical = new InterstitialAdController();
    #endregion

    #region RewardedAd
    public RewardedAdController rewarded = new RewardedAdController();
    #endregion
}



public enum AdUnitType
{
    Appopen,
    Banner,
    Interstitial,
    Rewarded,
}

public enum AdEventType
{
    Click,
    Load,
    FailToLoad,
    FailToShow,
    FailToOpen,
    Open,
    Impression,
    Paid,
    Close,
    CallLoad,
    Show,
}

public class GoogleMobileAdsConsentController
{
    /// <summary>
    /// If true, it is safe to call MobileAds.Initialize() and load Ads.
    /// </summary>
    public bool CanRequestAds => ConsentInformation.CanRequestAds();

    public void GatherConsent(Action<string> onComplete)
    {
        Debug.Log("Gathering consent.");

        var requestParameters = new ConsentRequestParameters
        {
            // False means users are not under age.
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = new ConsentDebugSettings
            {
                // For debugging consent settings by geography.
                DebugGeography = DebugGeography.Disabled,
                // https://developers.google.com/admob/unity/test-ads
                TestDeviceHashedIds = AdsController.TestDeviceIds,
            }
        };

        // Combine the callback with an error popup handler.
        onComplete = (onComplete == null)
            ? UpdateErrorPopup
            : onComplete + UpdateErrorPopup;

        // The Google Mobile Ads SDK provides the User Messaging Platform (Google's
        // IAB Certified consent management platform) as one solution to capture
        // consent for users in GDPR impacted countries. This is an example and
        // you can choose another consent management platform to capture consent.
        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {
            // Enable the change privacy settings button.
            UpdatePrivacyButton();

            if (updateError != null)
            {
                onComplete(updateError.Message);
                return;
            }

            // Determine the consent-related action to take based on the ConsentStatus.
            if (CanRequestAds)
            {
                // Consent has already been gathered or not required.
                // Return control back to the user.
                onComplete(null);
                return;
            }

            // Consent not obtained and is required.
            // Load the initial consent request form for the user.
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
            {
                UpdatePrivacyButton();
                if (showError != null)
                {
                    // Form showing failed.
                    if (onComplete != null)
                    {
                        onComplete(showError.Message);
                    }
                }
                // Form showing succeeded.
                else if (onComplete != null)
                {
                    onComplete(null);
                }
            });
        });
    }

    /// <summary>
    /// Shows the privacy options form to the user.
    /// </summary>
    /// <remarks>
    /// Your app needs to allow the user to change their consent status at any time.
    /// Load another form and store it to allow the user to change their consent status
    /// </remarks>
    public void ShowPrivacyOptionsForm(Action<string> onComplete)
    {
        Debug.Log("Showing privacy options form.");

        // combine the callback with an error popup handler.
        onComplete = (onComplete == null)
            ? UpdateErrorPopup
            : onComplete + UpdateErrorPopup;

        ConsentForm.ShowPrivacyOptionsForm((FormError showError) =>
        {
            UpdatePrivacyButton();
            if (showError != null)
            {
                // Form showing failed.
                if (onComplete != null)
                {
                    onComplete(showError.Message);
                }
            }
            // Form showing succeeded.
            else if (onComplete != null)
            {
                onComplete(null);
            }
        });
    }

    /// <summary>
    /// Reset ConsentInformation for the user.
    /// </summary>
    public void ResetConsentInformation()
    {
        ConsentInformation.Reset();
        UpdatePrivacyButton();
    }

    void UpdatePrivacyButton()
    {
    }

    void UpdateErrorPopup(string message)
    {
    }

}
