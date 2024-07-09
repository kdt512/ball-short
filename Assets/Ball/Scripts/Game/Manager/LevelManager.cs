using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Bottle> listBottle = new List<Bottle>();

    [SerializeField] protected GameObject BottlePrefab;
    [SerializeField] protected GameObject fxWin;

    [SerializeField] protected float spacePerTubeY;

    [SerializeField] protected float distanceBetweenTube;

    [SerializeField] protected float distanceBetweenSide;

    [SerializeField] private float minSizeCam;


    private Bottle BottlePending;
    private LevelElement levelElement;
    private bool isTransfer;
    private float wTube;
    private int countTube;
    private int starClaimInLevel;
    public LayerMask layerBottle;

    public int StarClaimInLevel
    {
        get => starClaimInLevel;
        set => starClaimInLevel = value;
    }


    private void Update()
    {
        if (isTransfer || UIManager.Instance.IsAnyUI()) return;
        if (Input.GetMouseButtonDown(0))
        {
            var collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), layerBottle);
            if (collider != null)
            {
                var Bottle = collider.GetComponent<Bottle>();
                if (Bottle != null)
                {
                    OnClickBottle(Bottle);
                }
            }
        }
    }


    private void Start()
    {
        InitLevel();
    }

    private void InitData()
    {
        StarClaimInLevel = 3;
        fxWin.gameObject.SetActive(false);
    }


    private void Resize(float widthBottle, float distanceBetweenBottle, float distanceBetweenSide,
        int maxCountBottleInRow)
    {
        Camera.main.orthographicSize = Mathf.Max(((widthBottle * maxCountBottleInRow +
                                                   distanceBetweenBottle * (maxCountBottleInRow - 1) +
                                                   distanceBetweenSide * 2) / Camera.main.aspect) / 2f, minSizeCam);

        EventDispatcher.PostEvent(EventId.UPDATE_BACKGROUND);
    }


   


    private bool IsWin()
    {
        return listBottle.All(x => x.IsDoneBottle || x.GetCountItems() <= 0);
    }


    private void MoveUp(Bottle Bottle, Action callback = null)
    {
        SoundManager.Instance.Play(SoundType.BALL_TOP);
        var topItem = Bottle.GetTopItem();
        var targetMove = Bottle.GetTargetPoint();
        topItem.MoveTo(targetMove.position, () =>
        {
            topItem.transform.position = targetMove.position;
            callback?.Invoke();
        });
    }


    private void MoveDown(Bottle Bottle, Action callback = null)
    {
        var topItem = Bottle.GetTopItem();
        topItem.SetBodyRig(RigidbodyType2D.Dynamic);
        topItem.EnableCol(true);
        callback?.Invoke();
        DOVirtual.DelayedCall(.25f, () => { SoundManager.Instance.Play(SoundType.BALL_DOWN); });
    }


    private void Transfer(Item Item, Bottle pendingBottle, Bottle targetBottle, Action callback = null)
    {
        isTransfer = true;
        pendingBottle.GetSecondItem()?.SetLock(false);
        Item.MoveTo(targetBottle.GetTargetPoint().position, () =>
        {
            Item.transform.position = targetBottle.GetTargetPoint().position;

            targetBottle.SetParentForItem(Item);
            pendingBottle.RemoveItem(Item);
            targetBottle.AddItem(Item);

            isTransfer = false;

            MoveDown(targetBottle, callback);
        });
    }


    protected void SpawnTube()
    {
        levelElement = MapManager.Instance.dataLevels.GetLevel(DataManager.CurrentNormalLevel);

        if (levelElement != null)
        {
            countTube =
                levelElement.countTube;
            var spriteTube = DataProvider.Instance.BottleData.GetItemDataById(DataManager.ICurrentIdBottle)
                .spriteInGame;
            wTube = spriteTube.rect.width / spriteTube.pixelsPerUnit;
            List<Vector3> postion = SetupPositionTube(countTube, wTube, out var maxCountInRow);

            var baseLevel = MapManager.Instance.GetLevel(DataManager.CurrentNormalLevel);
            if (baseLevel != null)
            {
                for (int j = 0; j < countTube; j++)
                {
                    Bottle Bottle = Instantiate(BottlePrefab, transform).GetComponent<Bottle>();
                    Bottle.transform.position = postion[j];
                    listBottle.Add(Bottle);
                    var balls = new List<int>();
                    if (j < baseLevel.mdLevels.Count)
                    {
                        balls = baseLevel.mdLevels[j].valueBall;
                    }

                    Bottle.OnInit(balls, j, levelElement.isHideObject);
                }
            }


            Resize(wTube, distanceBetweenTube, distanceBetweenSide, maxCountInRow);
        }
    }

    public void AddTube()
    {
        countTube++;

        Bottle Bottle = Instantiate(BottlePrefab, transform).GetComponent<Bottle>();
        listBottle.Add(Bottle);
        Bottle.OnInit(new List<int>(), listBottle.Count - 1, levelElement.isHideObject);

        List<Vector3> postion = SetupPositionTube(countTube, wTube, out var maxCountInRow);

        for (int i = 0; i < listBottle.Count; i++)
        {
            listBottle[i].transform.position = postion[i];
        }

        Resize(wTube, distanceBetweenTube, distanceBetweenSide, maxCountInRow);
        UIManager.Instance.gameplayUI.ChangeIconAddTube(false);
    }

    protected List<Vector3> SetupPositionTube(int countTube, float wTube, out int maxCountInRow)
    {
        var result = new List<Vector3>();

        if (countTube < 6)
        {
            var minPoint = transform.position - new Vector3((countTube - 1) * (wTube + distanceBetweenTube) / 2f, 0);
            for (int i = 0; i < countTube; i++)
            {
                result.Add(minPoint + i * new Vector3(wTube + distanceBetweenTube, 0));
            }

            maxCountInRow = countTube;
        }
        else
        {
            maxCountInRow = Mathf.CeilToInt(countTube / 2f);
            var minPoint = new Vector2(transform.position.x - (maxCountInRow - 1) * (wTube + distanceBetweenTube) / 2f,
                transform.position.y + spacePerTubeY);
            for (int i = 0; i < maxCountInRow; i++)
            {
                result.Add(minPoint + i * new Vector2(wTube + distanceBetweenTube, 0));
            }

            minPoint = new Vector2(
                transform.position.x - (countTube - maxCountInRow - 1) * (wTube + distanceBetweenTube) / 2f,
                transform.position.y - spacePerTubeY);
            for (int i = 0; i < countTube - maxCountInRow; i++)
            {
                result.Add(minPoint + i * new Vector2(wTube + distanceBetweenTube, 0));
            }
        }

        return result;
    }
    public void OnClickBottle(Bottle Bottle)
    {
        if (GameManager.Instance.StateGame == StateGame.WAIT)
        {
            GameManager.Instance.StateGame = StateGame.PLAY;
        }

        if (BottlePending)
        {
            if (BottlePending != Bottle)
            {
                BottlePending.IsPending = false;
                if (Bottle.IsDoneBottle)
                {
                    MoveDown(BottlePending);
                    BottlePending = null;
                }
                else if (Bottle.IsCanMoveTo(BottlePending.GetTopItem()))
                {
                    Transfer(BottlePending.GetTopItem(), BottlePending, Bottle, () =>
                    {
                        if (IsWin())
                        {
                            SoundManager.Instance.Play(SoundType.FINISH_GAME);
                            DOVirtual.DelayedCall(.3f, () => { fxWin.gameObject.SetActive(true); });
                            DOVirtual.DelayedCall(.6f, () =>
                            {
                                UIManager.Instance.OpenUI<WinPopup>(DialogType.WIN);
                                Debug.LogError("Win");
                            });
                        }
                    });
                    BottlePending = null;
                }
                else
                {
                    MoveDown(BottlePending);
                    BottlePending = Bottle;
                    MoveUp(Bottle, () => { Bottle.IsPending = true; });
                }
            }
            else if (Bottle.IsPending)
            {
                BottlePending.IsPending = false;
                BottlePending = null;
                MoveDown(Bottle);
            }
        }
        else if (Bottle.GetTopItem() && !Bottle.IsDoneBottle)
        {
            BottlePending = Bottle;
            Bottle.IsPending = false;
            MoveUp(Bottle, () => { Bottle.IsPending = true; });
        }
    }
    public void ResetLevel()
    {
        for (int i = 0; i < listBottle.Count; i++)
        {
            Destroy(listBottle[i].gameObject);
        }

        listBottle.Clear();
    }

    public void InitLevel()
    {
        InitData();
        SpawnTube();
    }

    public void PlayLevel()
    {
    }
}