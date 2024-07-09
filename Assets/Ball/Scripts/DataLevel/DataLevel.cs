using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/DataLevel", fileName = "DataLevel")]
public class DataLevel : ScriptableObject
{
    public List<LevelElement> dataLevels;


    public LevelElement GetLevel(int idLevel)
    {
        return dataLevels.FirstOrDefault(x => x.idLevel == idLevel);
    }
}

[System.Serializable]
public class LevelElement
{
    public string nameLevel;
    public int idLevel;
    public int countTube;
    public int countAddTube;
    public int countObjectDifferent;
    public int countStarReward;
    public int claimStar;
    public float timePlayLevel;
    public bool isHideObject;
}