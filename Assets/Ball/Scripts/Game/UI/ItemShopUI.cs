using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//DONE

public class ItemShopUI : MonoBehaviour
{
    [SerializeField] private Image imgBg;

    [SerializeField] private GameObject objBottles;

    [SerializeField] private List<Image> listImgBottle = new List<Image>();

    [SerializeField] private GameObject objBalls;

    [SerializeField] private List<Image> listImgBall = new List<Image>();

    [SerializeField] private GameObject objTick;

    [SerializeField] private GameObject objLock;

    private BackgroundData.ItemData _backgroundData;
    private BottleData.ItemData _BottleData;
    private BallData.ItemData _ballData;

    private int _id;
    private bool isLock;


    public void SetId(int id)
    {
        _id = id;
    }


    public void UIBackgroundData(BackgroundData.ItemData backgroundData)
    {
        _backgroundData = backgroundData;
        if (_backgroundData != null)
        {
            imgBg.sprite = _backgroundData.spriteItemShop;
        }
    }


    public void UIBottleData(BottleData.ItemData BottleData)
    {
        _BottleData = BottleData;
        if (_BottleData != null)
        {
            for (int i = 0; i < listImgBottle.Count; i++)
            {
                listImgBottle[i].sprite = _BottleData.spriteItemShop;
            }
        }
    }


    public void UIBallData(BallData.ItemData ballData)
    {
        _ballData = ballData;
        if (_ballData != null)
        {
            for (int i = 0; i < listImgBall.Count; i++)
            {
                listImgBall[i].sprite = _ballData.listSpriteBalls[i];
            }
        }
    }


    public void SetShow(ItemShopTypeEnum itemShopTypeEnum)
    {
        switch (itemShopTypeEnum)
        {
            case ItemShopTypeEnum.BACKGROUND:
                imgBg.gameObject.SetActive(true);
                objBottles.SetActive(false);
                objBalls.SetActive(false);
                gameObject.SetActive(_backgroundData != null);
                break;
            case ItemShopTypeEnum.Bottle:
                imgBg.gameObject.SetActive(false);
                objBottles.SetActive(true);
                objBalls.SetActive(false);
                gameObject.SetActive(_BottleData != null);
                break;
            case ItemShopTypeEnum.BALL:
                imgBg.gameObject.SetActive(false);
                objBottles.SetActive(false);
                objBalls.SetActive(true);
                gameObject.SetActive(_ballData != null);
                break;
            default:
                imgBg.gameObject.SetActive(true);
                objBottles.SetActive(false);
                objBalls.SetActive(false);
                gameObject.SetActive(_backgroundData != null);
                break;
        }
    }


    public void SetTick(ItemShopTypeEnum itemShopTypeEnum)
    {
        switch (itemShopTypeEnum)
        {
            case ItemShopTypeEnum.BACKGROUND:
                objTick.SetActive(DataManager.ICurrentIdBackground == _id);
                break;
            case ItemShopTypeEnum.Bottle:
                objTick.SetActive(DataManager.ICurrentIdBottle == _id);
                break;
            case ItemShopTypeEnum.BALL:
                objTick.SetActive(DataManager.ICurrentIdItem == _id);
                break;
            default:
                objTick.SetActive(DataManager.ICurrentIdBackground == _id);
                break;
        }
    }


    public void SetLock(ItemShopTypeEnum itemShopTypeEnum)
    {
        switch (itemShopTypeEnum)
        {
            case ItemShopTypeEnum.BACKGROUND:
                isLock = !DataManager.ListUnlockIdBackground.Contains(_id);
                break;
            case ItemShopTypeEnum.Bottle:
                isLock = !DataManager.ListUnlockIdBottle.Contains(_id);
                break;
            case ItemShopTypeEnum.BALL:
                isLock = !DataManager.ListUnlockIdItem.Contains(_id);
                break;
            default:
                isLock = true;
                break;
        }

        objLock.SetActive(isLock);
    }


    public void OnClickItemShop()
    {
        if (!isLock)
        {
            UIManager.Instance.GetUI<ShopUI>(DialogType.SHOP).OnClickItemShopUI(_id);
        }
    }
}