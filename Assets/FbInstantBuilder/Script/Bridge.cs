using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Security.AccessControl;
using UnityEngine.Events;

public class Bridge : MonoBehaviour
{
    #region webgl function
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void PreloadAds(string bannerId, string interstitialId, string rewardInterstitialId, string rewardVideoId);

    [DllImport("__Internal")]
    private static extern void ShowIntertitialAds(string callbackName);

    [DllImport("__Internal")]
    private static extern void ShowRewardVideoAds(string type);

    [DllImport("__Internal")]
    private static extern void ShowRewardInterstitialAds(string callbackName);

    [DllImport("__Internal")]
    private static extern void ShowBanner();

    [DllImport("__Internal")]
    private static extern void HideBanner();

    [DllImport("__Internal")]
    private static extern void ShareScreenShot(string imageBase64);

    [DllImport("__Internal")]
    private static extern void SharePlayLink();

    [DllImport("__Internal")]
    private static extern void Invite();

    [DllImport("__Internal")]
    private static extern bool IsLoadFbSDK();

    [DllImport("__Internal")]
    private static extern void PurchaseAnItem(string itemId);

    [DllImport("__Internal")]
    private static extern string GetPlatform(); //Can be "IOS", "ANDROID", "WEB" or "MOBILE_WEB".

    [DllImport("__Internal")]
    private static extern void SaveData(string key, string data);

    [DllImport("__Internal")]
    private static extern string LoadData(string key);

    [DllImport("__Internal")]
    private static extern void SaveDataAndFlush(string key, string data);

#endif
    #endregion
    private static string BANNER_ADS_ID = "";
    private static string INTERSTITIAL_ADS_ID = "";
    private static string REWARD_VIDEO_ADS_ID = "";
    private static string REWARD_INTERSTITIAL_ADS_ID = "";

    public static string PLATFORM_IOS = "IOS";
    public static string PLATFORM_WEB = "WEB";
    public static string PLATFORM_ANDROID = "ANDROID";
    public static string platfrom = "";

    private UnityAction<bool> completedAction;
    private UnityAction callbackAction;

    private Action<bool> purchaseCallback;
    private Action<bool> shareCallback;

    public static Bridge Instance { get; private set; }

    StringEventInvoker getPlayerNameInvoker;
    NormalEventInvoker gamePauseInvoker;
    NormalEventInvoker gameResumeInvoker;

    TakeScreenshotURP screenShot;

    public TakeScreenshotURP ScreenShoter
    {
        get { return screenShot; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Bridge Awake");
            Instance = this;
            DontDestroyOnLoad(gameObject);

            UpdateAdIdFromSetting();
#if UNITY_WEBGL && !UNITY_EDITOR
            LoadPlayerData();
            PreloadAds(BANNER_ADS_ID, INTERSTITIAL_ADS_ID, REWARD_INTERSTITIAL_ADS_ID, REWARD_VIDEO_ADS_ID);            
#endif
            if (!EventManager.IsInitial) EventManager.Initialize();
            getPlayerNameInvoker = gameObject.AddComponent<StringEventInvoker>();
            EventManager.AddInvoker(StringEventName.GetPlayerName, getPlayerNameInvoker);

            gamePauseInvoker = gameObject.AddComponent<NormalEventInvoker>();
            EventManager.AddInvoker(NormalEventName.GamePause, gamePauseInvoker);

            gameResumeInvoker = gameObject.AddComponent<NormalEventInvoker>();
            EventManager.AddInvoker(NormalEventName.GameResume, gameResumeInvoker);

            screenShot = gameObject.GetComponent<TakeScreenshotURP>();
        }
    }

    public void LoadPlayerData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadData(Constans.CURRENT_DATA);
#endif
    }

    //
    public void UpdateAdIdFromSetting()
    {
        var fbInstantSetting = Resources.Load<FbInstantSetting>(FbInstantConst.FBINSTANT_ASSET_NAME);
        if (fbInstantSetting != null)
        {
            BANNER_ADS_ID = fbInstantSetting.BannerId;
            INTERSTITIAL_ADS_ID = fbInstantSetting.InterstitialId;
            REWARD_INTERSTITIAL_ADS_ID = fbInstantSetting.RewardedInterstitialId;
            REWARD_VIDEO_ADS_ID = fbInstantSetting.RewardedId;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Bridge Start");
#if UNITY_WEBGL && !UNITY_EDITOR
        Hello();
#endif
    }

    private void OnDestroy()
    {
        EventManager.RemoveInvoker(StringEventName.GetPlayerName, getPlayerNameInvoker);
        EventManager.RemoveInvoker(NormalEventName.GameResume, gameResumeInvoker);
        EventManager.RemoveInvoker(NormalEventName.GamePause, gamePauseInvoker);
    }


    #region Public method

    public void ShareFbScreenShot(string base64Image)
    {
        Debug.Log("Bridge ShareFbScreenShot");
#if UNITY_WEBGL && !UNITY_EDITOR
        ShareScreenShot(base64Image);
#else
        Debug.Log("ShareScreenShot not webgl");
#endif
    }

    public void ShareFbPlayLink(Action<bool> callback = null)
    {
        print("ShareFbPlayLink");
        shareCallback = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        SharePlayLink();
#else
        Debug.Log("ShareFbPlayLink not webgl");
        if (callback != null) callback(true);
#endif

    }

    public void InviteFb()
    {
        print("InviteFb");
#if UNITY_WEBGL && !UNITY_EDITOR
        Invite();
#else
        Debug.Log("InviteFb not webgl");
#endif
    }

    public bool IsLoadedFbSDK()
    {
        Debug.Log("Bridge IsLoadedFbSDK");
#if UNITY_WEBGL && !UNITY_EDITOR
         return IsLoadFbSDK();
#endif
        return false;
    }

    public void ShowFbInterstitalAds(string placement, UnityAction callback = null, bool require = false)
    {
        Debug.Log("Bridge ShowFbInterstitalAds");
        callbackAction = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        gamePauseInvoker.CallEvent();
        ShowIntertitialAds("OnShowInterstitialAds");
#else
        callbackAction.Invoke();
#endif
    }

    public void ShowFbRewardInterstitalAds(string placement, UnityAction<bool> completed = null, UnityAction callback = null)
    {
        Debug.Log("Bridge ShowFbRewardInterstitalAds");
        completedAction = completed;
        callbackAction = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        gamePauseInvoker.CallEvent();
        ShowRewardInterstitialAds("OnShowRewardInterstitialAds");
#endif
    }

    public void ShowFbRewardVideoAds(string placement, UnityAction<bool> completed = null, UnityAction callback = null, bool require = false)
    {
        Debug.Log("Bridge ShowFbRewardVideoAds");
        completedAction = completed;
        callbackAction = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        gamePauseInvoker.CallEvent();
        ShowRewardVideoAds("OnShowRewardVideoAds");
#else
        Debug.Log("Bridge ShowFbRewardVideoAds not webgl");
        OnShowRewardVideoAds(0);
#endif
    }

    public void ShowFbBanner()
    {
        Debug.Log("Bridge ShowFbBanner");
#if UNITY_WEBGL && !UNITY_EDITOR
        ShowBanner();
#else
        Debug.Log("Bridge ShowFbBanner not webgl");
#endif
    }

    public void HideFbBanner()
    {
        Debug.Log("Bridge HideFbBanner");
#if UNITY_WEBGL && !UNITY_EDITOR
        HideBanner();
#else
        Debug.Log("Bridge HideBanner not webgl");
#endif
    }

    public void PurchaseFbItem(string itemId, Action<bool> callback)
    {
        print("PurchaseFbItem " + itemId);
        purchaseCallback = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        PurchaseAnItem(itemId);
        return;
#else
        print("PurchaseFbItem Not webgl");
        callback.Invoke(true);
#endif
    }

    public string GetFbPlatform()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (platfrom == "")
        {
            platfrom = GetPlatform();
        }
        return platfrom;
#endif
        return "";
    }

    public void SaveFbData(string key, string data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveData(key, data);
        return;
#else
        print("SaveFbData Not webgl");
#endif
    }

    public void SaveFbDataAndFlush(string key, string data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveDataAndFlush(key, data);
#else
        print("SaveFbData Not webgl");
#endif
    }

    public string LoadFbData(string key)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadData(key);
#else
        print("LoadData Not webgl");
#endif
        return "";
    }

    public bool IsPlatformIOS()
    {
        if (platfrom == "")
            platfrom = GetFbPlatform();
        return platfrom == PLATFORM_IOS;
    }
    #endregion
    #region Callback
    public void OnShowInterstitialAds(int accept)
    {
        Debug.Log("OnShowInterstitialAds " + (accept == 1 ? "true" : "false"));
        callbackAction?.Invoke();
        gameResumeInvoker.CallEvent();
    }

    public void OnShowRewardInterstitialAds(int accept)
    {
        Debug.Log("OnShowRewardInterstitialAds " + (accept == 1 ? "true" : "false"));
        if (accept == 1 && completedAction != null)
        {
            completedAction.Invoke(true);
            callbackAction?.Invoke();
        }
        else if (accept == 2 && completedAction != null) // user reject
        {
            completedAction.Invoke(false);
            callbackAction?.Invoke();

        }
        else
        {
            //Commons.ShowDialog("NOTIFY", "Ads not ready. Try again later!", () => { callbackAction?.Invoke(); });
        }
        gameResumeInvoker.CallEvent();
    }

    public void OnShowRewardVideoAds(int accept)
    {
        Debug.Log("OnShowRewardVideoAds " + (accept == 1 ? "true" : "false"));
        if (accept == 1 && completedAction != null)
        {
            completedAction.Invoke(true);
            callbackAction?.Invoke();
        }
        else if (accept == 2 && completedAction != null) // user reject
        {
            completedAction.Invoke(false);
            callbackAction?.Invoke();

        }
        else
        {
           // Commons.ShowDialog("NOTIFY", "Ads not ready. Try again later!", () => { callbackAction?.Invoke(); });
        }
        gameResumeInvoker.CallEvent();
    }

    public void OnGetPlayerName(string name)
    {
        Debug.Log("OnGetPlayerName " + name);
        getPlayerNameInvoker.CallEvent(name);
    }

    public void OnPurchaseCallback(int success)
    {
        print("Bridge OnPurchaseCallback " + success);
        //DebugCanvas.Instance?.ShowDebugMessage("Bridge OnPurchaseCallback " + success);
        purchaseCallback?.Invoke(success == 1 ? true : false);
    }

    public void OnConsumePurchaseCallback(string productId)
    {
        print("Bridge OnConsumePurchaseCallback " + productId);
        // todo add item for user

    }

    public void OnConsumeRemainPurchase(string productId)
    {
        print("Bridge OnConsumeRemainPurchase " + productId);
        //todo update Shop
        //Commons.LoadShopItem(productId);
    }

    public void ShowDebugInGame(string message)
    {
        print("ShowDebugInGame " + message);
        //DebugCanvas.Instance?.ShowDebugMessage(message);
    }

    public void OnShareCallback(int success)
    {
        shareCallback?.Invoke(success == 1 ? true : false);
    }

    public void OnPauseGame()
    {
        gamePauseInvoker.CallEvent();
        //Commons.ShowDialog("NOTIFY", "Do you want to continue playing the game?", () =>
        //{
        //    gameResumeInvoker.CallEvent();
        //});
    }

    public void OnLoadDataCompletely(string str)
    {
        string[] arrs = str.Split("|");

        if (arrs.Length == 2)
        {
            string key = arrs[0];
            string data = arrs[1];
            Debug.Log($"Brigde OnLoadDataCompletely {key} {data}");
            //Commons.OnLoadDataCompletely(key, data);
        }
        else
        {
            Debug.Log($"Brigde OnLoadDataCompletely Error data = {str}");
        }
    }

    #endregion
}
