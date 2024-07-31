public class CompleteTutorialPopup : BasePopup
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
        DataManager.IsFullTutorial = true;
        UIManager.Instance.CloseUI<CompleteTutorialPopup>(DialogType.COMPLETE_TUTORIAL);
    }
}