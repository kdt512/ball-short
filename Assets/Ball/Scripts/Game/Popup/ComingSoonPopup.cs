using DG.Tweening;

public class ComingSoonPopup : BasePopup
{
	public void OnClickLevel()
	{
		SoundManager.Instance.Play(SoundType.CLICK);
		DataManager.CurrentNormalLevel = MapManager.Instance.dataLevels.dataLevels.Count-1;

        GameManager.LoadGame();
        DOVirtual.DelayedCall(.7f, () =>
		{
		UIManager.Instance.OpenUI<BasePopup>(DialogType.LEVEL);
			UIManager.Instance.CloseUI<ComingSoonPopup>(DialogType.COMMING_SOON);
		});
    }
}