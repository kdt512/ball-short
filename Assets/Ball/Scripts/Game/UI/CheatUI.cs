using TMPro;
using UnityEngine;

public class CheatUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputStar;
    [SerializeField] private TMP_InputField inputCoin;
    [SerializeField] private TMP_InputField inputLevel;

    public void OnClickStar()
    {
        if (!string.IsNullOrEmpty(inputStar.text))
        {
            DataManager.STAR = int.Parse(inputStar.text);
            EventDispatcher.PostEvent(EventId.UPDATE_STAR);
        }
    }

    public void OnClickCoin()
    {
        if (!string.IsNullOrEmpty(inputCoin.text))
        {
            DataManager.COIN = int.Parse(inputCoin.text);
            EventDispatcher.PostEvent(EventId.UPDATE_COIN);
        }
    }

    public void OnClickLevel()
    {
        if (!string.IsNullOrEmpty(inputLevel.text))
        {
            var lv = int.Parse(inputLevel.text) - 1;
            if (lv < Constans.NUM_LEVELS)
            {
                DataManager.CurrentNormalLevel = lv;
                if (DataManager.UnlockNormalLevel < lv)
                {
                    for (int i = 0; i < lv - DataManager.UnlockNormalLevel; i++)
                    {
                        DataManager.AddList(Constans.LIST_STAR_LEVEL_RECEIVED, 0);
                    }

                    DataManager.UnlockNormalLevel = lv;
                }

                GameManager.LoadGame();
            }
        }
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    public void OpenAdInspector()
    {
        //AdmodManager.Instance.OpenAdInspector();
    }
}