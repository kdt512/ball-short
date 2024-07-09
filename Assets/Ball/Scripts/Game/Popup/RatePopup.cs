using System.Collections.Generic;
using UnityEngine;

public class RatePopup : BasePopup
{
	[SerializeField]
	private List<GameObject> listStar = new List<GameObject>();

	private const string _androidAppUrl =
		"https://play.google.com/store/apps/details?id=com.ball.sort.puzzle.tiger.games";

	private const string _iosAppUrl = "https://apps.apple.com/app/ball-sort-colors-puzzle/id1644651248";
	private       int    _starRate  = 5;



	public void OnClickStar(int starNum)
	{
		//Reset star
		for (int i = 0; i < listStar.Count; i++)
		{
			listStar[i].SetActive(false);
		}

		for (int i = 0; i < starNum; i++)
		{
			listStar[i].SetActive(true);
		}

		_starRate = starNum;
	}



	public void OnClickClose()
	{
		SoundManager.Instance.Play(SoundType.CLICK);
		Close();
	}



	public void OnClickSubmit()
	{
		SoundManager.Instance.Play(SoundType.CLICK);
		DataManager.IsRate = true;

		var notiUI = UIManager.Instance.OpenUI<NotiPopup>(DialogType.POPUP_NOTI);
		notiUI.ShowAsInfo("Rating", "Thanks for your rating!");

#if UNITY_ANDROID
		Application.OpenURL(_androidAppUrl);
#elif UNITY_IPHONE
        Application.OpenURL(_iosAppUrl);
#endif
	}
}