using System;
using TMPro;
using UnityEngine;
//DONE
public class NotiPopup : BasePopup
{
	[SerializeField]
	private TextMeshProUGUI _titleTxt;

	[SerializeField]
	private TextMeshProUGUI _messageTxt;

	private Action _action = null;



	public void OnClickButton()
	{
		SoundManager.Instance.Play(SoundType.CLICK);
		Close();
		_action?.Invoke();
	}



	public void ShowAsInfo(string title, string message, Action action = null)
	{
		_titleTxt.text   = title;
		_messageTxt.text = message;
		_action          = action;
	}
}