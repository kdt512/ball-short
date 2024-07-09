using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "ScriptableObject/BackgroundData")]
public class BackgroundData : ScriptableObject
{
	public List<ItemData> listBackgroundData = new List<ItemData>();

	[System.Serializable]
	public class ItemData
	{
		public int    bgId;
		public Sprite spriteItemShop;
		public Sprite spriteInGame;
	}



	public ItemData GetItemDataById(int bgId)
	{
		return listBackgroundData.Count <= 0 ? null : listBackgroundData.FirstOrDefault(x => x.bgId == bgId);
	}



	public List<ItemData> GetItemsData()
	{
		return listBackgroundData;
	}
}