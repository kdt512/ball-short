using System.Collections.Generic;

public class MDBaseLevel
{
	public List<MDLevel> baseLevels = new List<MDLevel>();
}

public class MDLevel
{
	public List<MDLevelElement> mdLevels = new List<MDLevelElement>();
}

public class MDLevelElement
{
	public List<int> valueBall = new List<int>();
}