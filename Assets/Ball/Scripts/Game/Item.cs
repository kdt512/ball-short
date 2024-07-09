using System;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;

    public int ID
    {
        get => id;
        set => id = value;
    }

    private bool isLock;

    public bool IsLock
    {
        get => isLock;
        set => isLock = value;
    }


    private Tween tweenMove;

    [SerializeField] protected SpriteRenderer skin;

    [SerializeField] private Rigidbody2D rig;

    [SerializeField] private CircleCollider2D col;

    [SerializeField] private GameObject objLock;
    [SerializeField] private GameObject objUnlock;

    private void Awake()
    {
        EventDispatcher.RegisterListener(EventId.UPDATE_Item, UpdateItem);
    }

    private void UpdateItem(object obj)
    {
        var itemData = DataProvider.Instance.ballData.GetItemDataById(DataManager.ICurrentIdItem);
        if (itemData != null)
        {
            skin.sprite = itemData.listSpriteBalls[Mathf.Clamp(ID, 0, itemData.listSpriteBalls.Count - 1)];
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener(EventId.UPDATE_Item, UpdateItem);
    }


    public void SetLock(bool islock2)
    {
        isLock = islock2;
        objLock.SetActive(isLock);
        objUnlock.SetActive(!isLock);
    }

    public void SetBodyRig(RigidbodyType2D bodyType)
    {
        rig.bodyType = bodyType;
    }


    public void EnableCol(bool isEnable)
    {
        col.enabled = isEnable;
    }


    public void MoveTo(Vector2 endPos, Action callback = null)
    {
        tweenMove?.Kill();
        SetBodyRig(RigidbodyType2D.Static);
        EnableCol(false);
        var startPos = transform.position;
        var duration = Utils.GetDurationMove(startPos, endPos);
        tweenMove = transform.DOMove(endPos, duration).SetEase(Ease.Linear).OnComplete(() => { callback?.Invoke(); });
    }


    public void OnInit(int id2)
    {
        this.ID = id2;
        UpdateItem(null);
    }
}