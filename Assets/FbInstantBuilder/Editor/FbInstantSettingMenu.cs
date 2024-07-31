using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TigerGames
{
    public class FbInstantSettingMenu : UnityEditor.Editor
    {
        [MenuItem("TigerGames/FbInstant Setting")]
        private static void SettingAd()
        {
            Debug.Log("Setting FbInstant");

            if (!Directory.Exists(FbInstantConst.FBINSTANT_RESOURCES_PATH))
            {
                Directory.CreateDirectory(FbInstantConst.FBINSTANT_RESOURCES_PATH);
            }

            var fbInstantSettings = Resources.Load<FbInstantSetting>(FbInstantConst.FBINSTANT_ASSET_NAME);
            Debug.Log($"fbInstantSettings {FbInstantSetting.SETTINGS_ASSET_PATH}");
            if (fbInstantSettings == null)
            {
                //Debug.LogWarning(TigerGamesAdConstants.AD_SETTING_NAME + " can't be found, creating a new one...");
                fbInstantSettings = CreateInstance<FbInstantSetting>();
                AssetDatabase.CreateAsset(fbInstantSettings, FbInstantSetting.SETTINGS_ASSET_PATH);
                fbInstantSettings = Resources.Load<FbInstantSetting>(FbInstantConst.FBINSTANT_ASSET_NAME);
            }

            Selection.activeObject = fbInstantSettings;
        }
    }

}

