using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class LevelInfoList : MonoBehaviour, IEnhancedScrollerDelegate
{
	public EnhancedScroller         scroller;
	public EnhancedScrollerCellView cellViewPrefab;

	public int numberOfCellsPerRow = 4;



	void Start()
	{
		scroller.Delegate = this;
		LoadData();
	}



	private void LoadData()
	{
		scroller.ReloadData();
	}



#region EnhancedScroller Handlers

	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return Mathf.CeilToInt((float)Constans.NUM_LEVELS / (float)numberOfCellsPerRow);
	}



	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 250f;
	}



	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		LevelCellView cellView = scroller.GetCellView(cellViewPrefab) as LevelCellView;

		// data index of the first sub cell
		var di = dataIndex * numberOfCellsPerRow;

		if (cellView != null)
		{
			cellView.name = "Cell " + (di).ToString() + " to " + ((di) + numberOfCellsPerRow - 1).ToString();

			// pass in a reference to our data set with the offset for this cell
			cellView.SetData(di);

			// return the cell to the scroller
			return cellView;
		}

		return null;
	}

#endregion
}