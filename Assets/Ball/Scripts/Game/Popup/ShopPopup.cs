using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : BasePopup
{
    [SerializeField] private BackgroundData _backgroundData;

    [SerializeField] private BottleData _BottleData;

    [SerializeField] private BallData _ballData;

    [SerializeField] private ItemShopUI _itemShopUIPrefab;

    [SerializeField] private Transform contents;

    [SerializeField] private TextMeshProUGUI txtCoinGift;
    [SerializeField] private TextMeshProUGUI txtCoinAds;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private Button bntReward;
    [SerializeField] private Image imageBntDisable;
    [SerializeField] private float maxTimeBntDisable;
    [SerializeField] private float currentTimeBntDisable;

    private ItemShopTypeEnum currentItemShopType;
    private List<ItemShopUI> listItemShopUI = new List<ItemShopUI>();

    public Button btnBuy;


    private void Start()
    {
        var maxData = Mathf.Max(_backgroundData.GetItemsData().Count, _BottleData.GetItemsData().Count,
            _ballData.GetItemsData().Count);
        for (int i = 0; i < maxData; i++)
        {
            var itemShopUI = Instantiate(_itemShopUIPrefab, contents);
            itemShopUI.SetId(i);
            SetupItemShopUI(itemShopUI, i);
            listItemShopUI.Add(itemShopUI);
        }

        SetShow();

        txtCoinGift.text = DataProvider.Instance.coinShop.ToString();
        txtCoinAds.text = DataProvider.Instance.coinAds.ToString();

        txtTitle.text = "THEME";
    }
    private void OnEnable()
    {
        if (DateTime.Now.Hour - DataManager.TimeDisableReward >= 1)
        {
            if (DataManager.IsDisableReward)
            {
                DataManager.IsDisableReward = false;
            }
        }
        imageBntDisable.fillAmount = 0;
        imageBntDisable.gameObject.SetActive(false);
        bntReward.enabled = true;
        maxTimeBntDisable = 3;
        currentItemShopType = 0;
    }
    IEnumerator ActionClickReward()
    {
        imageBntDisable.gameObject.SetActive(true);
        imageBntDisable.fillAmount = 1;
        bntReward.enabled = false;
        while (currentTimeBntDisable < maxTimeBntDisable)
        {
            yield return null;
            currentTimeBntDisable += Time.deltaTime;
            imageBntDisable.fillAmount = 1 - (currentTimeBntDisable / maxTimeBntDisable);
        }
        bntReward.enabled = true;
        currentTimeBntDisable = 0;
        imageBntDisable.gameObject.SetActive(false);

    }
    public override void OnInit()
    {
        base.OnInit();

        SetShow();
    }


    private void SetShow()
    {
        for (int i = 0; i < listItemShopUI.Count; i++)
        {
            listItemShopUI[i].SetShow(currentItemShopType);
            listItemShopUI[i].SetTick(currentItemShopType);
            listItemShopUI[i].SetLock(currentItemShopType);
        }

        switch (currentItemShopType)
        {
            case ItemShopTypeEnum.BACKGROUND:
                btnBuy.interactable = _backgroundData.GetItemsData()
                    .Any(x => !DataManager.ListUnlockIdBackground.Contains(x.bgId));
                break;
            case ItemShopTypeEnum.Bottle:
                btnBuy.interactable = _BottleData.GetItemsData()
                    .Any(x => !DataManager.ListUnlockIdBottle.Contains(x.id));
                break;
            case ItemShopTypeEnum.BALL:
                btnBuy.interactable = _ballData.GetItemsData()
                    .Any(x => !DataManager.ListUnlockIdItem.Contains(x.skinId));
                break;
            default:
                btnBuy.interactable = _backgroundData.GetItemsData()
                    .Any(x => !DataManager.ListUnlockIdBackground.Contains(x.bgId));
                break;
        }
    }


    private void SetupItemShopUI(ItemShopUI itemShopUI, int id)
    {
        var backgroundItemData = _backgroundData.GetItemDataById(id);
        var BottleItemData = _BottleData.GetItemDataById(id);
        var ballItemData = _ballData.GetItemDataById(id);

        itemShopUI.UIBackgroundData(backgroundItemData);
        itemShopUI.UIBottleData(BottleItemData);
        itemShopUI.UIBallData(ballItemData);
    }


    public void OnClickButtonType(int id)
    {
        currentItemShopType = (ItemShopTypeEnum)id;
        SetShow();
        switch (id)
        {
            case 0:
                txtTitle.text = "THEME";
                break;
            case 1:
                txtTitle.text = "TUBE";
                break;
            case 2:
                txtTitle.text = "BALL";
                break;
            default:
                txtTitle.text = "THEME";
                break;
        }
    }


    public void OnClickOpenGift()
    {
        if (DataManager.COIN >= DataProvider.Instance.coinShop)
        {
            var openGiftUI = UIManager.Instance.OpenUI<OpenGiftPopup>(DialogType.OPEN_GIFT);
            openGiftUI.SetGift(false);
            openGiftUI.SetData(currentItemShopType);
        }
        else
        {
            var notiUI = UIManager.Instance.OpenUI<NotiPopup>(DialogType.POPUP_NOTI);
            notiUI.ShowAsInfo("Oops!", "Coin is not enough, please watch ads to get more coins!");
        }
    }


    public void OnClickGetCoinAds()
    {
        if (DataManager.IsDisableReward)
        {
            var notiUI = UIManager.Instance.OpenUI<NotiPopup>(DialogType.POPUP_NOTI);
            notiUI.ShowAsInfo("NOTIFY!", "You click too match request ads! This button was disabled in 1 hour");

        }
        else
        {
            Debug.LogError("Click");
            //DataOneLoad.Instance.indexClickReward++;
            //if (DataOneLoad.Instance.indexClickReward == 1)
            //{
            //    DataOneLoad.Instance.TimeDisable();
            //}
            //if (DataOneLoad.Instance.indexClickReward == 5)
            //{
            //    if (DataOneLoad.Instance.timeClickReward > 0)
            //    {
            //        DataManager.TimeDisableReward = DateTime.Now.Hour;
            //        DataManager.IsDisableReward = true;
            //    }
            //}
            DataManager.TimeDisableReward = DateTime.Now.Hour;
            DataManager.IsDisableReward = true;
            StartCoroutine(ActionClickReward());

            AdsController.Instance.ShowRewardedVideoAd("GetCoinAds", (t) =>
            {
                if (!t) return;

                DataManager.COIN += DataProvider.Instance.coinAds;
                EventDispatcher.PostEvent(EventId.UPDATE_COIN);
            });
        }
    }
    public void OnClickItemShopUI(int id)
    {
        bool isReset;

        switch (currentItemShopType)
        {
            case ItemShopTypeEnum.BACKGROUND:
                isReset = DataManager.ICurrentIdBackground != id;
                DataManager.ICurrentIdBackground = id;
                EventDispatcher.PostEvent(EventId.UPDATE_BACKGROUND);
                break;
            case ItemShopTypeEnum.Bottle:
                isReset = DataManager.ICurrentIdBottle != id;
                DataManager.ICurrentIdBottle = id;
                DataManager.ICurrentIdBottleBlink = id;
                EventDispatcher.PostEvent(EventId.UPDATE_Bottle);
                EventDispatcher.PostEvent(EventId.UPDATE_Bottle_BLINK);
                break;
            case ItemShopTypeEnum.BALL:
                isReset = DataManager.ICurrentIdItem != id;
                DataManager.ICurrentIdItem = id;
                EventDispatcher.PostEvent(EventId.UPDATE_Item);
                break;
            default:
                isReset = DataManager.ICurrentIdBackground != id;
                DataManager.ICurrentIdBackground = id;
                EventDispatcher.PostEvent(EventId.UPDATE_BACKGROUND);
                break;
        }

        if (isReset)
        {
            SetShow();
        }
    }
}

public enum ItemShopTypeEnum
{
    BACKGROUND,
    Bottle,
    BALL
}