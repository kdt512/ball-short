
using System.IO;
using UnityEngine;

public class FbInstantSetting : ScriptableObject
{
    public static readonly string SETTINGS_ASSET_PATH = Path.Combine(FbInstantConst.FBINSTANT_RESOURCES_PATH, FbInstantConst.FBINSTANT_ASSET_NAME+".asset");

    [Header("Fb Instant Ads")]
    [Tooltip("Add your ads unit id")]

    public string BannerId = string.Empty;
    public string InterstitialId = string.Empty;
    public string RewardedInterstitialId = string.Empty;
    public string RewardedId = string.Empty;
}
