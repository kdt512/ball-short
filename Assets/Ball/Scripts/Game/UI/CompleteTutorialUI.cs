public class CompleteTutorialUI : BaseUI
{
    public override void OnClick(int index)
    {
        base.OnClick(index);
        switch (index)
        {
            case 0:
                Click_Close();
                break;
        }
    }

    protected void Click_Close()
    {
        //TrackingManager.Instance.LogEventCompleteTutorial();
        DataManager.IsFullTutorial = true;
        UIManager.Instance.CloseUI<CompleteTutorialUI>(DialogType.COMPLETE_TUTORIAL);
    }
}