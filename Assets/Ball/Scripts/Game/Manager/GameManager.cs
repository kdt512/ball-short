using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] protected StateGame stateGame;

    public StateGame StateGame
    {
        get => stateGame;
        set => stateGame = value;
    }
    public bool isTutorialInLevel;
    public int indexRateUs;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        isTutorialInLevel = false;
        indexRateUs= DataManager.CurrentNormalLevel;
        DOVirtual.DelayedCall(1, () =>
        {
            //AdmodManager.Instance.ShowBanner();
        });

#if UNITY_EDITOR
        if (!DataProvider.isPlayLoading)
        {
            SceneManager.LoadScene(0);
        }
#endif
    }
    public static void LoadGame()
    {
        SceneManager.LoadScene(1);

    }
}

public enum StateGame
{
    WAIT,
    PLAY
}