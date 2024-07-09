using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager
{
    public static long TotalBannerAdsValue
    {
        get => long.Parse(PlayerPrefs.GetString(Constans.TOTAL_BANNER_ADS_VALUE, "0"));
        set => PlayerPrefs.SetString(Constans.TOTAL_BANNER_ADS_VALUE, value.ToString());
    }

    public static long TotalInterAdsValue
    {
        get => long.Parse(PlayerPrefs.GetString(Constans.TOTAL_INTER_ADS_VALUE, "0"));
        set => PlayerPrefs.SetString(Constans.TOTAL_INTER_ADS_VALUE, value.ToString());
    }

    public static long TotalRewardAdsValue
    {
        get => long.Parse(PlayerPrefs.GetString(Constans.TOTAL_REWARD_ADS_VALUE, "0"));
        set => PlayerPrefs.SetString(Constans.TOTAL_REWARD_ADS_VALUE, value.ToString());
    }

    public static long TotalAppOpenAdsValue
    {
        get => long.Parse(PlayerPrefs.GetString(Constans.TOTAL_APP_OPEN_ADS_VALUE, "0"));
        set => PlayerPrefs.SetString(Constans.TOTAL_APP_OPEN_ADS_VALUE, value.ToString());
    }

    public static string UserId
    {
        get => PlayerPrefs.GetString(Constans.IDFA, "");
        set => PlayerPrefs.SetString(Constans.IDFA, value);
    }

    public static int COIN
    {
        get => PlayerPrefs.GetInt(Constans.COIN, 0);
        set => PlayerPrefs.SetInt(Constans.COIN, value);
    }

    public static int STAR
    {
        get => PlayerPrefs.GetInt(Constans.STAR, 0);
        set => PlayerPrefs.SetInt(Constans.STAR, value);
    }

    public static bool IsSoundOn
    {
        get => GetBool(Constans.SOUND, true);
        set => SetBool(Constans.SOUND, value);
    }

    public static bool IsMusicOn
    {
        get => GetBool(Constans.MUSIC, true);
        set => SetBool(Constans.MUSIC, value);
    }

    public static int CountRate
    {
        get => PlayerPrefs.GetInt(Constans.COUNT_RATE, 5);
        set => PlayerPrefs.SetInt(Constans.COUNT_RATE, value);
    }

    public static int ICurrentIdBackground
    {
        get => PlayerPrefs.GetInt(Constans.CURRENT_ID_BACKGROUND, 0);
        set => PlayerPrefs.SetInt(Constans.CURRENT_ID_BACKGROUND, value);
    }

    public static int ICurrentIdBottle
    {
        get => PlayerPrefs.GetInt(Constans.CURRENT_ID_BOTTLE, 0);
        set => PlayerPrefs.SetInt(Constans.CURRENT_ID_BOTTLE, value);
    }

    public static int ICurrentIdBottleBlink
    {
        get => PlayerPrefs.GetInt(Constans.CURRENT_ID_BOTTLE_BLINK, 0);
        set => PlayerPrefs.SetInt(Constans.CURRENT_ID_BOTTLE_BLINK, value);
    }

    public static int ICurrentIdItem
    {
        get => PlayerPrefs.GetInt(Constans.CURRENT_ID_ITEM, 0);
        set => PlayerPrefs.SetInt(Constans.CURRENT_ID_ITEM, value);
    }

    public static int CurrentNormalLevel
    {
        get => PlayerPrefs.GetInt(Constans.CURRENT_LEVEL, 0);
        set => PlayerPrefs.SetInt(Constans.CURRENT_LEVEL, value);
    }

    public static int UnlockNormalLevel
    {
        get => PlayerPrefs.GetInt(Constans.UNLOCK_LEVEL, 0);
        set => PlayerPrefs.SetInt(Constans.UNLOCK_LEVEL, value);
    }

    public static int AddingTimeFreeNum
    {
        get => PlayerPrefs.GetInt(Constans.ADDING_TIME_FREE_NUM, 0);
        set => PlayerPrefs.SetInt(Constans.ADDING_TIME_FREE_NUM, value);
    }

    public static int CountTimerStar
    {
        get => PlayerPrefs.GetInt(Constans.COUNT_TIME_STAR, 5);
        set => PlayerPrefs.SetInt(Constans.COUNT_TIME_STAR, value);
    }

    public static int TimeDisableReward
    {
        get => PlayerPrefs.GetInt(Constans.TIME_DISABLE_REWARD, DateTime.Now.Hour);
        set => PlayerPrefs.SetInt(Constans.TIME_DISABLE_REWARD, value);
    }

    public static bool IsDisableReward
    {
        get => GetBool(Constans.IS_DISABLE_REWARD, false);
        set => SetBool(Constans.IS_DISABLE_REWARD, value);
    }

    public static bool IsRate
    {
        get => GetBool(Constans.IS_RATE, false);
        set => SetBool(Constans.IS_RATE, value);
    }

    public static bool IsFullTutorial
    {
        get => GetBool(Constans.IS_TUTORIAL, false);
        set => SetBool(Constans.IS_TUTORIAL, value);
    }

    public static bool IsNoAds
    {
        get => GetBool(Constans.NO_ADS, false);
        set => SetBool(Constans.NO_ADS, value);
    }


    public static List<int> ListUnlockIdBackground =>
        GetList<int>(Constans.UNLOCK_ID_BACKGROUND, new List<int>() { 0 });

    public static List<int> ListUnlockIdBottle => GetList<int>(Constans.UNLOCK_ID_BOTTLE, new List<int>() { 0 });

    public static List<int> ListUnlockIdItem => GetList<int>(Constans.UNLOCK_ID_ITEM, new List<int>() { 0 });

    public static List<int> ListStarLevelReceived => GetList<int>(Constans.LIST_STAR_LEVEL_RECEIVED, new List<int>());


    private static bool GetBool(string key, bool defaultValue = false)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key) != 0;
        }
        else
        {
            PlayerPrefs.SetInt(key, defaultValue ? 1 : 0);
            return defaultValue;
        }
    }


    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }


    private static List<T> GetList<T>(string key, List<T> defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return JsonConvert.DeserializeObject<List<T>>(PlayerPrefs.GetString(key));
        }
        else
        {
            PlayerPrefs.SetString(key, JsonConvert.SerializeObject(defaultValue));
            return defaultValue;
        }
    }


    public static void AddList<T>(string key, T value)
    {
        var result = GetList(key, new List<T>());
        result.Add(value);
        PlayerPrefs.SetString(key, JsonConvert.SerializeObject(result));
    }

    public static void SetList<T>(string key, int index, T value)
    {
        var result = GetList(key, new List<T>());
        if (index < result.Count)
        {
            result[index] = value;
        }

        PlayerPrefs.SetString(key, JsonConvert.SerializeObject(result));
    }
}