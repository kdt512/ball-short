using DG.Tweening;

public class ComingSoonUI : BaseUI
{
	public void OnClickLevel()
	{
		SoundManager.Instance.Play(SoundType.CLICK);
		DataManager.CurrentNormalLevel = GenMap.Instance.dataLevels.dataLevels.Count-1;

        GameManager.LoadGame();
        DOVirtual.DelayedCall(.7f, () =>
		{
		UIManager.Instance.OpenUI<BaseUI>(DialogType.LEVEL);
			UIManager.Instance.CloseUI<ComingSoonUI>(DialogType.COMMING_SOON);
		});
    }
}