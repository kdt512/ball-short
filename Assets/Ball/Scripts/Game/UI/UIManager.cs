using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<BaseUI> listBaseUI = new List<BaseUI>();

    [SerializeField] private Transform contents;

    private Dictionary<DialogType, BaseUI> uiDic = new Dictionary<DialogType, BaseUI>();
    private BaseUI currentUI;

    [HideInInspector] public GameplayUI gameplayUI;

    [SerializeField] protected GameObject gameplayUIPrefab;


    private void Start()
    {
        OpenGameplay();
    }


    private void OpenGameplay()
    {
        if (gameplayUI == null)
        {
            gameplayUI = Instantiate(gameplayUIPrefab, transform).GetComponent<GameplayUI>();
        }

        gameplayUI.Open();
    }

    public void CloseGameplay()
    {
        gameplayUI?.Close();
    }


    public T OpenUI<T>(DialogType dialogType) where T : BaseUI
    {
        if (uiDic.TryGetValue(dialogType, out var ui))
        {
            if (!ui.IsOpen())
            {
                if (currentUI != ui && currentUI != null)
                {
                    currentUI.Close();
                }

                ui.Open();
                currentUI = ui;
            }
        }
        else
        {
            if (currentUI != null)
            {
                currentUI.Close();
            }

            currentUI = Instantiate(listBaseUI[(int)dialogType], contents);
            currentUI.Open();
            uiDic.Add(dialogType, currentUI);
        }

        return currentUI as T;
    }

    public void CloseUI<T>(DialogType type) where T : BaseUI
    {
        T t = GetUI<T>(type);
        t.Close();
        ClearUI();
    }


    public void ClearUI()
    {
        currentUI = null;
    }


    public bool IsAnyUI()
    {
        return currentUI != null;
    }


    public T GetUI<T>(DialogType dialogType) where T : BaseUI
    {
        if (currentUI == null || currentUI._dialogType != dialogType)
        {
            return null;
        }

        return currentUI as T;
    }
}

public enum DialogType
{
    SETTING,
    SHOP,
    WIN,
    LEVEL,
    NO_ADS,
    OPEN_GIFT,
    POPUP_NOTI,
    COMMING_SOON,
    RATE,
    TUTORIAL,
    COMPLETE_TUTORIAL
}