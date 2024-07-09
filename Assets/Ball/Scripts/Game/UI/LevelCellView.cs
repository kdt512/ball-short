using EnhancedUI.EnhancedScroller;


public class LevelCellView : EnhancedScrollerCellView
{
	public LevelRowCellView[] rowCellViews;



	public void SetData(int startingIndex)
	{
		for (var i = 0; i < rowCellViews.Length; i++)
		{
			rowCellViews[i].SetData(startingIndex + i);
		}
	}
}