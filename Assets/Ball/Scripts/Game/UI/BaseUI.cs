using UnityEngine;
//DONE

public class BaseUI : MonoBehaviour
{
	public DialogType _dialogType;



	public virtual void Open()
	{
		gameObject.SetActive(true);
		GameManager.Instance.StateGame = StateGame.WAIT;
		OnInit();
	}



	public virtual void Close()
	{
		gameObject.SetActive(false);
      
        UIManager.Instance.ClearUI();
	}



	public virtual void OnInit()
	{
	}



	public virtual void OnClick(int index)
	{
	}



	public bool IsOpen()
	{
		return gameObject.activeInHierarchy;
	}
}