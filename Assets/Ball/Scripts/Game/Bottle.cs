using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Transform targetPoint;

    [SerializeField] private ParticleSystem particleFxStar;

    [SerializeField] private GameObject ItemPrefab;

    [SerializeField] private Transform contents;

    [SerializeField] private SpriteRenderer BottleRenderer;
    [SerializeField] private SpriteRenderer BottleBlinkRenderer;

    public List<Item> listItems = new List<Item>();
    public List<int> listMapIDItem = new List<int>();

    public Vector2 sizeItem;
    public Item topItem;
    public int siblingIDLevel;
    public int idBottle;
    private bool isPending;
    private bool isDoneBottle;

    public bool IsDoneBottle
    {
        get => isDoneBottle;
        set => isDoneBottle = value;
    }

    public bool IsPending
    {
        get => isPending;
        set => isPending = value;
    }

    private void Awake()
    {
        EventDispatcher.RegisterListener(EventId.UPDATE_Bottle, UpdateBottle);
        EventDispatcher.RegisterListener(EventId.UPDATE_Bottle, UpdateBottleBlink);
    }

    private void UpdateBottle(object obj)
    {
        var itemData = DataProvider.Instance.BottleData.GetItemDataById(DataManager.ICurrentIdBottle);
        if (itemData != null)
        {
            BottleRenderer.sprite = itemData.spriteInGame;
        }
    }

    private void UpdateBottleBlink(object obj)
    {
        var itemData = DataProvider.Instance.BottleData.GetItemDataById(DataManager.ICurrentIdBottleBlink);
        if (itemData != null)
        {
            BottleBlinkRenderer.sprite = itemData.spriteComplete;
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener(EventId.UPDATE_Bottle, UpdateBottle);
        EventDispatcher.RemoveListener(EventId.UPDATE_Bottle, UpdateBottleBlink);
    }


    private void SpawnItem(bool isHideObj)
    {
        if (listMapIDItem.Count <= 0) return;
        float timerDelay = 0;
        float size = -1.2f;
        for (int i = 0; i < listMapIDItem.Count; i++)
        {
            Item Item = Instantiate(ItemPrefab, contents).GetComponent<Item>();
            Item.transform.localPosition = new Vector3(0, size, 0);
            int index = i;
            Item.OnInit(listMapIDItem[index] - 1);
            Item.SetLock(isHideObj);
            listItems.Add(Item);
            size += 0.7f;
            timerDelay += .4f;
        }
    }


    public void OnInit(List<int> idItem, int idBottle2, bool isHideObj)
    {
        this.BottleBlinkRenderer.gameObject.SetActive(false);
        this.listMapIDItem = idItem;
        this.idBottle = idBottle2;
        SpawnItem(isHideObj);

        topItem = listItems.LastOrDefault();
        if (topItem)
        {
            topItem.SetLock(false);
        }

        UpdateBottle(null);
        UpdateBottleBlink(null);
    }


    public Item GetSecondItem()
    {
        return listItems.Count > 1 ? listItems[listItems.Count - 2] : null;
    }

    public Item GetTopItem()
    {
        return topItem;
    }


    public Transform GetTargetPoint()
    {
        return targetPoint;
    }


    public bool IsCanMoveTo(Item Item)
    {
        return topItem == null || (topItem.ID == Item.ID && listItems.Count < 4);
    }


    public bool IsCheckBottle()
    {
        for (var i = 1; i < listItems.Count; i++)
        {
            if (listItems[i].ID != listItems[0].ID)
            {
                return false;
            }
        }

        return listItems.Count >= 4 && listItems.All(x => !x.IsLock);
    }


    public void SetParentForItem(Item Item)
    {
        Item.transform.SetParent(contents);
    }


    public int GetCountItems()
    {
        return listItems.Count;
    }


    public void RemoveItem(Item Item)
    {
        if (listItems.Contains(Item))
        {
            listItems.Remove(Item);
            topItem = listItems.LastOrDefault();
        }
    }


    public void AddItem(Item Item)
    {
        if (!listItems.Contains(Item))
        {
            listItems.Add(Item);
        }

        topItem = listItems.LastOrDefault();

        if (!isDoneBottle && IsCheckBottle())
        {
            isDoneBottle = true;
            particleFxStar.gameObject.SetActive(true);
            particleFxStar.Play();
            BottleBlinkRenderer.gameObject.SetActive(true);
            SoundManager.Instance.Play(SoundType.FINISH_TUBE);
        }
    }
}