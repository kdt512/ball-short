using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRowCellView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private List<Image> listImgStar;

    [SerializeField] private GameObject content;

    [SerializeField] private GameObject _isLockPanel;

    [SerializeField] private Sprite _starBlack;

    [SerializeField] private Sprite _starGold;

    [SerializeField] private Button btnLevel;

    public int DataIndex;
    private bool isLock;


    public void SetData(int dataIndex)
    {
        isLock = dataIndex > DataManager.UnlockNormalLevel;
        content.SetActive(dataIndex < Constans.NUM_LEVELS);


        //Setup UI
        if (!isLock)
        {
            _isLockPanel.gameObject.SetActive(false);
            _levelText.text = (dataIndex + 1).ToString();
            if (dataIndex < DataManager.ListStarLevelReceived.Count)
            {
                for (int i = 0; i < listImgStar.Count; i++)
                {
                    listImgStar[i].sprite =
                        i < DataManager.ListStarLevelReceived[dataIndex] ? _starGold : _starBlack;
                }
            }
            else
            {
                
            }
        }
        else
        {
            _isLockPanel.gameObject.SetActive(true);
        }

        btnLevel.interactable = !isLock;

        DataIndex = dataIndex;
    }


    public void OnSelectLevel()
    {
        if (!isLock && DataIndex < Constans.NUM_LEVELS)
        {
            SoundManager.Instance.Play(SoundType.CLICK);
            DataManager.CurrentNormalLevel = DataIndex;
            GameManager.LoadGame();
        }
    }
}