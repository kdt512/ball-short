using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "ScriptableObject/BallData")]
public class BallData : ScriptableObject
{
	public List<ItemData> listBallData = new List<ItemData>();

	[System.Serializable]
	public class ItemData
	{
		public int          skinId;
		public Sprite       spriteItemShop;
		public List<Sprite> listSpriteBalls = new List<Sprite>();
	}



	public ItemData GetItemDataById(int id)
	{
		return listBallData.Count <= 0 ? null : listBallData.FirstOrDefault(x => x.skinId == id);
	}



	public List<ItemData> GetItemsData()
	{
		return listBallData;
	}
}