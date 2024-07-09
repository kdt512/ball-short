using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//DONE
public class GameplayUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textLevel;
    [SerializeField] protected TextMeshProUGUI textBntStarTime;
    [SerializeField] protected TextMeshProUGUI textStarReward;
    [SerializeField] private Image imgClock;
    [SerializeField] private Sprite spriteClockUnlock;
    [SerializeField] private Sprite spriteClockAds;
    [SerializeField] protected GameObject iconWatchTimeStar;
    [SerializeField] protected Transform targetStar;
    [SerializeField] protected Slider sliderTimer;
    [SerializeField] protected Animator animReward;
    [SerializeField] protected float timerStarSlider;
    [SerializeField] protected List<GameObject> listStarViews;
    [SerializeField] protected List<GameObject> listStarYeallow;
    [SerializeField] protected List<GameObject> listStarGray;
    [SerializeField] protected GameObject addTubeIcon;
    [SerializeField] protected Image addTubeImage;

    private int countTubeAdd = 1;

    [SerializeField] private GameObject objCheat;
    private int countTapCheat;
    private float timeDelayCheat;

    private void Awake()
    {
        EventDispatcher.RegisterListener(EventId.UPDATE_STAR, UpdateStar);
    }

    private void UpdateStar(object obj)
    {
        ChangeStarView();
        animReward.Rebind();
        animReward.enabled = DataManager.STAR >= 20;
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener(EventId.UPDATE_STAR, UpdateStar);
    }

    private void Update()
    {
        if (GameManager.Instance.StateGame == StateGame.PLAY)
        {
            if (timeDelayCheat > 0)
            {
                timeDelayCheat -= Time.deltaTime;
                if (timeDelayCheat < 0)
                {
                    countTapCheat = 0;
                }
            }


            timerStarSlider -= Time.deltaTime;
            sliderTimer.value = timerStarSlider;
            int value = (int)timerStarSlider;
            switch (value)
            {
                case 40:
                    if (LevelManager.Instance.StarClaimInLevel == 3)
                    {
                        listStarYeallow[2].gameObject.SetActive(false);
                        listStarGray[2].gameObject.SetActive(true);
                        LevelManager.Instance.StarClaimInLevel--;
                    }

                    break;
                case 20:
                    if (LevelManager.Instance.StarClaimInLevel == 2)
                    {
                        listStarYeallow[1].gameObject.SetActive(false);
                        listStarGray[1].gameObject.SetActive(true);
                        LevelManager.Instance.StarClaimInLevel--;
                    }

                    break;
                case 0:
                    if (LevelManager.Instance.StarClaimInLevel == 1)
                    {
                        listStarYeallow[0].gameObject.SetActive(false);
                        listStarGray[0].gameObject.SetActive(true);
                        LevelManager.Instance.StarClaimInLevel--;
                    }

                    break;
            }
        }
    }

    public void OnClickCheat()
    {
        timeDelayCheat = 1;
        countTapCheat++;
        if (countTapCheat > 10)
        {
            countTapCheat = 0;
            objCheat.SetActive(true);
        }
    }

    public void Open()
    {
    }

    private void Start()
    {
        OnInit();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ChangeIconAddTube(bool isActive)
    {
        addTubeIcon.SetActive(isActive);
        addTubeImage.color = new Color(1, 1, 1, isActive ? 1 : .5f);
    }

    private void StarRewardAnim(int valueStar)
    {
        for (var i = 0; i < valueStar; i++)
        {
            var index = i;
            listStarViews[index].SetActive(true);
            listStarViews[index].transform.DOMove(targetStar.position, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                listStarViews[index].SetActive(false);
            });
        }

        DOVirtual.DelayedCall(0.99f, () =>
        {
            DataManager.STAR += valueStar;
            ChangeStarView();
            animReward.enabled = DataManager.STAR >= 20 ? true : false;
        });
    }

    private void ChangeStarView()
    {
        int indexStarClaim = DataManager.STAR;
        StringBuilder sbStarClaim = new StringBuilder();
        sbStarClaim.Append(indexStarClaim);
        textStarReward.text = sbStarClaim.ToString();
    }

    protected void SetupTutorial()
    {
        if (DataManager.CurrentNormalLevel <= 1 && !GameManager.Instance.isTutorialInLevel &&
            !DataManager.IsFullTutorial)
        {
            UIManager.Instance.OpenUI<TutorialUI>(DialogType.TUTORIAL);
        }
    }

    protected virtual void OnInit()
    {
        timerStarSlider = 60;
        sliderTimer.value = timerStarSlider;
        gameObject.SetActive(true);
        int indexLevel = DataManager.CurrentNormalLevel;
        StringBuilder sbLevel = new StringBuilder();
        sbLevel.Append("LEVEL ");
        sbLevel.Append(indexLevel + 1);
        textLevel.text = sbLevel.ToString();

        ChangeIconAddTube(true);

        ChangeViewStarTime(false);

        ChangeStarView();

        for (int i = 0; i < listStarViews.Count; i++)
        {
            listStarViews[i].SetActive(false);
        }

        for (int i = 0; i < listStarYeallow.Count; i++)
        {
            listStarGray[i].gameObject.SetActive(false);
            listStarYeallow[i].gameObject.SetActive(true);
        }

        if (DataProvider.Instance.starClaim > 0)
        {
            var valueStarClaim = DataProvider.Instance.starClaim;
            if (DataManager.ListStarLevelReceived.Count >= DataManager.CurrentNormalLevel)
            {
                var index = Mathf.Max(DataManager.CurrentNormalLevel - 1, 0);
                if (DataManager.ListStarLevelReceived[index] <
                    DataProvider.Instance.starClaim)
                {
                    valueStarClaim = DataProvider.Instance.starClaim -
                                     DataManager.ListStarLevelReceived[index];
                    DataManager.SetList(Constans.LIST_STAR_LEVEL_RECEIVED, index,
                        DataProvider.Instance.starClaim);
                }
                else
                {
                    valueStarClaim = 0;
                }
            }
            else
            {
                DataManager.AddList(Constans.LIST_STAR_LEVEL_RECEIVED, DataProvider.Instance.starClaim);
            }

            if (valueStarClaim > 0)
            {
                StarRewardAnim(valueStarClaim);
            }

            DataProvider.Instance.starClaim = 0;
        }

        animReward.enabled = DataManager.STAR >= 20;

        SetupTutorial();

        CommingSoon();

        RateUs();

    }

    public int ScoreGame()
    {
        int score = (int)timerStarSlider * 5 * (DataManager.CurrentNormalLevel * 10 + 100) / 100;
        return score;
    }

    public virtual void OnClick(int index)
    {
        switch (index)
        {
            case 0:
                Click_Setting();
                break;
            case 1:
                Click_ResetLevel();
                break;
            case 2:
                Click_Star();
                break;
            case 3:
                Click_AddTube();
                break;
            case 4:
                Click_ClaimStar_Reward();
                break;
        }

        SoundManager.Instance.Play(SoundType.CLICK);
    }

    protected void CommingSoon()
    {
        if (MapManager.Instance.dataLevels.dataLevels.Count <= DataManager.CurrentNormalLevel)
        {
            UIManager.Instance.OpenUI<ComingSoonPopup>(DialogType.COMMING_SOON);
        }
    }
    protected void RateUs()
    {
        //if (DataOneLoad.RateUsIndex == 6 && !DataManager.IsRate)
        //{
        //    UIManager.Instance.OpenUI<RateUI>(DialogType.RATE);
        //}
    }
    private void ChangeViewStarTime(bool isDvd = false)
    {
        if (DataManager.CountTimerStar > 0)
        {
            int countStarTime = isDvd ? DataManager.CountTimerStar - 1 : DataManager.CountTimerStar;
            StringBuilder sbTimeStar = new StringBuilder();
            sbTimeStar.Append(countStarTime);
            textBntStarTime.text = sbTimeStar.ToString();
            iconWatchTimeStar.gameObject.SetActive(false);
            DataManager.CountTimerStar = countStarTime;
            imgClock.sprite = spriteClockUnlock;
        }

        if (DataManager.CountTimerStar <= 0)
        {
            textBntStarTime.gameObject.SetActive(false);
            iconWatchTimeStar.gameObject.SetActive(true);
            imgClock.sprite = spriteClockAds;
        }
        else
        {
            textBntStarTime.gameObject.SetActive(true);
            iconWatchTimeStar.gameObject.SetActive(false);
            imgClock.sprite = spriteClockUnlock;
        }
    }

    private void Click_Setting()
    {
        UIManager.Instance.OpenUI<SettingPopup>(DialogType.SETTING);
    }

    private void Click_ResetLevel()
    {
        GameManager.LoadGame();
    }

    private void Click_Star()
    {
        Action succes = () =>
        {
            timerStarSlider += 5;
            timerStarSlider = timerStarSlider >= 60 ? 60 : timerStarSlider;
            if (timerStarSlider >= 40)
            {
                LevelManager.Instance.StarClaimInLevel = 3;
                listStarYeallow[2].gameObject.SetActive(true);
                listStarGray[2].gameObject.SetActive(false);
            }
            else if (timerStarSlider >= 20 && timerStarSlider <= 40)
            {
                LevelManager.Instance.StarClaimInLevel = 2;
                listStarYeallow[1].gameObject.SetActive(true);
                listStarGray[1].gameObject.SetActive(false);
            }
            else if (timerStarSlider >= 0 && timerStarSlider <= 20)
            {
                LevelManager.Instance.StarClaimInLevel = 1;
                listStarYeallow[0].gameObject.SetActive(true);
                listStarGray[0].gameObject.SetActive(false);
            }
        };
        if (DataManager.CountTimerStar <= 0)
        {
            // watch reward ads
            Debug.LogError("RewardAds");
            //AdmodManager.Instance.ShowReward((t) =>
            //{
            //    DataManager.CountTimerStar = 6;
            //    sliderTimer.value = timerStarSlider;
            //    ChangeViewStarTime(true);
            //});

        }
        else
        {
            succes?.Invoke();

            sliderTimer.value = timerStarSlider;
            ChangeViewStarTime(true);
        }
    }

    private void Click_AddTube()
    {
        if (countTubeAdd > 0)
        {
            //AdmodManager.Instance.ShowReward((t) =>
            //{
            //    countTubeAdd--;
            //    LevelManager.Instance.AddTube();
            //});
        }
    }

    private void Click_ClaimStar_Reward()
    {
        //claim reward
        if (DataManager.STAR >= 20)
        {
            var giftOpen = UIManager.Instance.OpenUI<OpenGiftPopup>(DialogType.OPEN_GIFT);
            giftOpen.SetGift(true);
        }
    }
}