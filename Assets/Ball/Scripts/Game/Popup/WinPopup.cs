using Newtonsoft.Json;
using Spine.Unity;
using System;
using TMPro;
using UnityEngine;

public class WinPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI _congratulationTxt;

    [SerializeField] private TextMeshProUGUI _colectTxt;

    [SerializeField] private GameObject addStarButton;

    [SerializeField] private SkeletonGraphic _skeletonGraphicStar;
    Action shareFB;

    private int _starReceive;
    private bool isShowAds;


    void Start()
    {
        _starReceive = LevelManager.Instance.StarClaimInLevel;
        _skeletonGraphicStar.gameObject.SetActive(true);
        _skeletonGraphicStar.AnimationState.ClearTracks();
        switch (_starReceive)
        {
            case 1:
                _skeletonGraphicStar.AnimationState.SetAnimation(0, "Star 1", false);
                _congratulationTxt.text = "Nicely Done";
                addStarButton.gameObject.SetActive(true);
                break;
            case 2:
                _skeletonGraphicStar.AnimationState.SetAnimation(0, "Star 2", false);
                _congratulationTxt.text = "Good Job";
                addStarButton.gameObject.SetActive(true);

                break;
            case 3:
                _skeletonGraphicStar.AnimationState.SetAnimation(0, "Star 3", false);
                _congratulationTxt.text = "Excellent Work";
                addStarButton.gameObject.SetActive(false);

                break;
            default:
                _congratulationTxt.text = "You're done well";
                addStarButton.gameObject.SetActive(true);
                _skeletonGraphicStar.gameObject.SetActive(false);
                break;
        }
    }


    public override void OnClick(int index)
    {
        base.OnClick(index);
        switch (index)
        {
            case 0:
                Click_Continue();
                break;
            case 1:
                Click_Share();
                break;
            case 2:
                Click_CollectStar();
                break;
        }
    }

    private void Click_CollectStar()
    {
        //// watch ads
        SoundManager.Instance.Play(SoundType.CLICK);
        //AdmodManager.Instance.ShowReward((t) =>
        //{
        //    isShowAds = false;
        //    DataManager.CurrentNormalLevel++;
        //    if (DataManager.UnlockNormalLevel < DataManager.CurrentNormalLevel)
        //    {
        //        DataManager.UnlockNormalLevel = DataManager.CurrentNormalLevel;
        //        TrackingManager.Instance.LogEventLevelUp(DataManager.UnlockNormalLevel);
        //    }

        //    int index = LevelManager.Instance.StarClaimInLevel + 1;
        //    DataProvider.Instance.valueStarClaim = index;
        //    NextLevel();
        //});
        isShowAds = false;
        DataManager.CurrentNormalLevel++;
        if (DataManager.UnlockNormalLevel < DataManager.CurrentNormalLevel)
        {
            DataManager.UnlockNormalLevel = DataManager.CurrentNormalLevel;
            //TrackingManager.Instance.LogEventLevelUp(DataManager.UnlockNormalLevel);
        }

        int index = LevelManager.Instance.StarClaimInLevel + 1;
        DataProvider.Instance.starClaim = index;
        NextLevel();
    }

    private void Click_Continue()
    {
        SoundManager.Instance.Play(SoundType.CLICK);

        DataManager.CurrentNormalLevel++;
        if (DataManager.UnlockNormalLevel < DataManager.CurrentNormalLevel)
        {
            isShowAds = true;
            DataManager.UnlockNormalLevel = DataManager.CurrentNormalLevel;
            //TrackingManager.Instance.LogEventLevelUp(DataManager.UnlockNormalLevel);
        }

        DataProvider.Instance.starClaim = LevelManager.Instance.StarClaimInLevel;
        NextLevel();
    }

    private void Click_Share()
    {
        SoundManager.Instance.Play(SoundType.CLICK);

        // share
        //FBData data = new FBData();
        //data.score = UIManager.Instance.gameplayUI.ScoreGame();
        //data.descTitle = "Pool Ball Sort Challenge";
        //ShareFB(data.score, JsonConvert.SerializeObject(data), Click_Continue);
    }

    private void ShareFB(int score, string data, Action callback = null)
    {
        shareFB = callback;
#if UNITY_WEBGL && !UNITY_EDITOR
        //ShareTournament("Car Parker Challenge", score, data);
#else
        print("ShareFbTournament Not webgl");
        shareFB?.Invoke();
#endif
    }

    private void NextLevel()
    {
        //if (isShowAds && DataManager.UnlockNormalLevel >= Constans.SHOW_ADS_LEVEL)
        //{
        //    //AdmodManager.Instance.ShowInterstitial((t) => { GameManager.LoadGame(); });
        //}
        //else
        //{
        //    GameManager.LoadGame();
        //}
        GameManager.LoadGame();

        //DataOneLoad.RateUsIndex++;
    }
}