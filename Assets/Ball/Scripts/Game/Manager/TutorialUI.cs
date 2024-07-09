using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : BaseUI
{
    [SerializeField] protected GameObject tut1;
    [SerializeField] protected GameObject handTut1;

    [SerializeField] protected GameObject tut2;
    [SerializeField] protected GameObject bowTut2;
    [SerializeField] protected TextMeshProUGUI textMeshTut2;
    [SerializeField] protected int indexStepTut1;
    [SerializeField] protected int indexStepTut2;
    [SerializeField] protected int numberTut;
    [SerializeField] protected List<Transform> posTut1;
    [SerializeField] protected List<Transform> posTut2;
    [SerializeField] protected List<string> textTut2 = new List<string>();

    public override void OnInit()
    {
        GameManager.Instance.isTutorialInLevel = true;
        numberTut = DataManager.CurrentNormalLevel;
        handTut1.transform.position = posTut1[0].position;
        tut1.SetActive(DataManager.CurrentNormalLevel == 0 ? true : false);
        InitTut2();
    }

    private void Update()
    {
        if (numberTut == 0)
        {
            if (indexStepTut1 <= 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                        LevelManager.Instance.layerBottle);
                    if (collider != null)
                    {
                        var Bottle = collider.GetComponent<Bottle>();
                        if (Bottle != null && Bottle.idBottle == indexStepTut1)
                        {
                            LevelManager.Instance.OnClickBottle(Bottle);
                            indexStepTut1++;
                            Tut1();
                        }
                    }
                }
            }
        }
        else if (numberTut == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                indexStepTut2++;
                Tut2();
            }
        }
    }

    protected void Tut1()
    {
        switch (indexStepTut1)
        {
            case 0:
                handTut1.transform.position = posTut1[0].position;
                break;
            case 1:
                handTut1.transform.position = posTut1[1].position;
                break;
        }

        if (indexStepTut1 == 2)
        {
            UIManager.Instance.CloseUI<TutorialUI>(DialogType.TUTORIAL);
        }
    }

    protected void Tut2()
    {
        switch (indexStepTut2)
        {
            case 0:
                bowTut2.transform.position = posTut2[0].position;
                textMeshTut2.text = textTut2[0];

                break;
            case 1:
                bowTut2.transform.position = posTut2[1].position;
                textMeshTut2.text = textTut2[1];

                break;
            case 2:
                bowTut2.transform.position = posTut2[2].position;
                textMeshTut2.text = textTut2[2];

                break;
            case 3:
                bowTut2.transform.position = posTut2[3].position;
                textMeshTut2.text = textTut2[3];
                break;
            case 4:
                bowTut2.transform.position = posTut2[4].position;
                textMeshTut2.text = textTut2[4];
                LevelManager.Instance.AddTube();
                break;
        }

        if (indexStepTut2 == 5)
        {
            UIManager.Instance.CloseUI<TutorialUI>(DialogType.TUTORIAL);
            UIManager.Instance.OpenUI<CompleteTutorialUI>(DialogType.COMPLETE_TUTORIAL);
        }
    }

    protected void InitTut2()
    {
        bowTut2.transform.position = posTut2[0].position;
        tut2.SetActive(DataManager.CurrentNormalLevel == 0 ? false : true);
        textMeshTut2.text = textTut2[0];
    }
}