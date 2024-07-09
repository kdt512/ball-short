using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI txtCoin;



	private void Awake()
	{
		EventDispatcher.RegisterListener(EventId.UPDATE_COIN, UpdateCoin);
	}



	private void OnDestroy()
	{
		EventDispatcher.RemoveListener(EventId.UPDATE_COIN, UpdateCoin);
	}



	private void Start()
	{
		UpdateCoin(null);
	}



	private void UpdateCoin(object obj)
	{
		txtCoin.text = DataManager.COIN.ToString();
	}
}