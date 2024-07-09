using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
//DONE
public class OpenGiftPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI _Title;

    [SerializeField] private Sprite _cash;

    [SerializeField] private GameObject _box;

    [SerializeField] private GameObject _BgGift;

    [SerializeField] private Image _gift;

    [SerializeField] private GameObject _openingBoxAnim;

    [SerializeField] private SkeletonGraphic _skeletonGraphicBox;

    [SerializeField] private GameObject _lightGift;

    [SerializeField] private BackgroundData _backgroundData;

    [SerializeField] private BottleData _BottleData;

    [SerializeField] private BallData _ballData;

    private ItemShopTypeEnum currentShopTypeEnum;
    private IEnumerator coroutine;
    private bool isGift;

    private List<int> _bgAvailable = new();
    private List<int> _tubeAvailable = new();
    private List<int> _ballAvailable = new();


    public void SetGift(bool gift)
    {
        isGift = gift;
    }


    public override void OnInit()
    {
        base.OnInit();

        _bgAvailable = _backgroundData.GetItemsData()
            .Where(x => !DataManager.ListUnlockIdBackground.Contains(x.bgId))
            .Select(x => x.bgId).ToList();
        _tubeAvailable = _BottleData.GetItemsData()
            .Where(x => !DataManager.ListUnlockIdBottle.Contains(x.id))
            .Select(x => x.id).ToList();
        _ballAvailable = _ballData.GetItemsData()
            .Where(x => !DataManager.ListUnlockIdItem.Contains(x.skinId))
            .Select(x => x.skinId).ToList();

        _box.SetActive(true);
        _skeletonGraphicBox.AnimationState.ClearTracks();
        _openingBoxAnim.SetActive(false);
        _lightGift.SetActive(false);
        _gift.gameObject.SetActive(false);
        _BgGift.SetActive(false);
    }


    public void SetData(ItemShopTypeEnum itemShopTypeEnum)
    {
        currentShopTypeEnum = itemShopTypeEnum;
    }


    public void OnClickOpenBox()
    {
        SoundManager.Instance.Play(SoundType.CLICK);
        _box.SetActive(false);
        _openingBoxAnim.SetActive(true);
        _skeletonGraphicBox.AnimationState.SetAnimation(0, "animation", false);
        coroutine = WaitAndOpen(0.8f);
        StartCoroutine(coroutine);
    }


    public override void Close()
    {
        base.Close();

        SoundManager.Instance.Play(SoundType.CLICK);
        _lightGift.SetActive(false);
        _box.SetActive(false);

        if (!isGift)
        {
            UIManager.Instance.OpenUI<ShopPopup>(DialogType.SHOP);
        }
    }


    private IEnumerator WaitAndOpen(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (isGift)
        {
            OpenBox();
        }
        else
        {
            LuckyDice();
        }

        _lightGift.SetActive(true);
    }


    private void LuckyDice()
    {
        SoundManager.Instance.Play(SoundType.CLICK);
        isGift = true;
        int itemId = 0;
        switch (currentShopTypeEnum)
        {
            case ItemShopTypeEnum.BACKGROUND:
                itemId = _bgAvailable[Random.Range(0, _bgAvailable.Count)];
                DataManager.AddList(Constans.UNLOCK_ID_BACKGROUND, itemId);
                _gift.sprite = _backgroundData.GetItemDataById(itemId).spriteItemShop;
                break;
            case ItemShopTypeEnum.Bottle:
                itemId = _tubeAvailable[Random.Range(0, _tubeAvailable.Count)];
                DataManager.AddList(Constans.UNLOCK_ID_BOTTLE, itemId);
                _gift.sprite = _BottleData.GetItemDataById(itemId).spriteItemShop;
                _BgGift.SetActive(true);
                break;
            case ItemShopTypeEnum.BALL:
                itemId = _ballAvailable[Random.Range(0, _ballAvailable.Count)];
                DataManager.AddList(Constans.UNLOCK_ID_ITEM, itemId);
                _gift.sprite = _ballData.GetItemDataById(itemId).spriteItemShop;
                break;
            default:
                itemId = _bgAvailable[Random.Range(0, _bgAvailable.Count)];
                DataManager.AddList(Constans.UNLOCK_ID_BACKGROUND, itemId);
                _gift.sprite = _BottleData.GetItemDataById(itemId).spriteItemShop;
                break;
        }

        _gift.gameObject.SetActive(true);

        DataManager.COIN -= DataProvider.Instance.coinShop;
        EventDispatcher.PostEvent(EventId.UPDATE_COIN);
    }


    private void OpenBox()
    {
        int itemId = 0;

        List<int> rndArr = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 4, 5, 6 };
        int giftIndex = Random.Range(0, rndArr.Count);


        switch (rndArr[giftIndex])
        {
            case 1:
                _gift.sprite = _cash;
                DataManager.COIN += 50;
                _Title.SetText("+50");
                break;
            case 2:
                _gift.sprite = _cash;
                DataManager.COIN += 100;
                _Title.SetText("+100");
                break;
            case 3:
                _gift.sprite = _cash;
                DataManager.COIN += 200;
                _Title.SetText("+200");
                break;
            case 4:
                if (_bgAvailable.Count > 0)
                {
                    itemId = _bgAvailable[Random.Range(0, _bgAvailable.Count)];
                    DataManager.AddList(Constans.UNLOCK_ID_BACKGROUND, itemId);
                    _gift.sprite = _backgroundData.GetItemDataById(itemId).spriteItemShop;
                    _Title.SetText("NEW THEME!!!");
                }
                else
                {
                    _gift.sprite = _cash;
                    DataManager.COIN += 50;
                    _Title.SetText("+50");
                }

                break;
            case 5:
                if (_tubeAvailable.Count > 0)
                {
                    itemId = _tubeAvailable[Random.Range(0, _tubeAvailable.Count)];
                    DataManager.AddList(Constans.UNLOCK_ID_BOTTLE, itemId);
                    _gift.sprite = _BottleData.GetItemDataById(itemId).spriteItemShop;
                    _BgGift.gameObject.SetActive(true);
                    _Title.SetText("NEW TUBE!!!");
                }
                else
                {
                    _gift.sprite = _cash;
                    DataManager.COIN += 50;
                    _Title.SetText("+50");
                }

                break;
            case 6:
                if (_ballAvailable.Count > 0)
                {
                    itemId = _ballAvailable[Random.Range(0, _ballAvailable.Count)];
                    DataManager.AddList(Constans.UNLOCK_ID_ITEM, itemId);
                    _gift.sprite = _ballData.GetItemDataById(itemId).spriteItemShop;
                    _Title.SetText("NEW BALL!!!");
                }
                else
                {
                    _gift.sprite = _cash;
                    DataManager.COIN += 50;
                    _Title.SetText("+50");
                }

                break;
        }

        _gift.gameObject.SetActive(true);
        DataManager.STAR -= 20;
        EventDispatcher.PostEvent(EventId.UPDATE_STAR);
    }
}