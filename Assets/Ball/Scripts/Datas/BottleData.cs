using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BottleData", menuName = "ScriptableObject/BottleData")]
public class BottleData : ScriptableObject
{
	public List<ItemData> listBottleData = new List<ItemData>();

	[System.Serializable]
	public class ItemData
	{
		public int    id;
		public Sprite spriteItemShop;
		public Sprite spriteInGame;
		public Sprite spriteComplete;
	}



	public ItemData GetItemDataById(int id)
	{
		return listBottleData.Count <= 0 ? null : listBottleData.FirstOrDefault(x => x.id == id);
	}



	public List<ItemData> GetItemsData()
	{
		return listBottleData;
	}
}