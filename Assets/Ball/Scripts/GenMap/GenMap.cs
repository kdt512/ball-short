using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class GenMap : Singleton<GenMap>
{
    public DataLevel dataLevels;

    MDBaseLevel baseLevel;

    protected override void Awake()
    {
        base.Awake();
        GenMapInit();

    }

    private void GenMapInit()
    {
        baseLevel = new MDBaseLevel();

        if (!PlayerPrefs.HasKey(Constans.DATA_LEVEL))
        {
            baseLevel.baseLevels = new List<MDLevel>();
            /// set data in all level 
            for (int i = 0; i < dataLevels.dataLevels.Count; i++)
            {
                MDLevel data = new MDLevel();

                if (i == 0)
                {
                    data.mdLevels.Add(new MDLevelElement() { valueBall = new List<int> { 1, 1 } });
                    data.mdLevels.Add(new MDLevelElement() { valueBall = new List<int> { 1, 1 } });
                    for (int a = 0; a < dataLevels.dataLevels[i].countAddTube; a++)
                    {
                        data.mdLevels.Add(new MDLevelElement() { });
                    }

                    baseLevel.baseLevels.Add(data);
                }
                else if (i == 1)
                {
                    data.mdLevels.Add(new MDLevelElement() { valueBall = new List<int> { 1, 1, 2, 2 } });
                    data.mdLevels.Add(new MDLevelElement() { valueBall = new List<int> { 2, 2, 1, 1 } });
                    for (int a = 0; a < dataLevels.dataLevels[i].countAddTube; a++)
                    {
                        data.mdLevels.Add(new MDLevelElement() { });
                    }

                    baseLevel.baseLevels.Add(data);
                }
                else
                {
                    int add = dataLevels.dataLevels[i].countObjectDifferent;
                    List<int> valueInLevel = new List<int>();


                    for (int j = 1; j <= add; j++)
                    {
                        for (int n = 0; n < 4; n++)
                        {
                            valueInLevel.Add(j);
                        }
                    }

                    Utils.ShuffleDuplicate(valueInLevel);

                    for (int c = 0; c < add; c++)
                    {
                        for (int d = 0; d < 4; d += 4)
                        {
                            var value = valueInLevel.Skip(c * 4).Take(4).ToList();
                            data.mdLevels.Add(new MDLevelElement() { valueBall = value });
                        }
                    }

                    for (int z = 0;
                         z < (dataLevels.dataLevels[i].countTube - dataLevels.dataLevels[i].countObjectDifferent +
                              dataLevels.dataLevels[i].countAddTube);
                         z++)
                    {
                        data.mdLevels.Add(new MDLevelElement() { });
                    }

                    //LoadDataPerLevel(dataLevels.dataLevels[i], data);
                    baseLevel.baseLevels.Add(data);
                }
            }
            string str = JsonConvert.SerializeObject(baseLevel);
            PlayerPrefs.SetString(Constans.DATA_LEVEL, str);
        }
        else
        {
            baseLevel = JsonConvert.DeserializeObject<MDBaseLevel>(PlayerPrefs.GetString(Constans.DATA_LEVEL));
        }
    }


    protected void LoadDataPerLevel(LevelElement levelElement, MDLevel data)
    {
       
    }



    protected void AddObjectInLevel(LevelElement levelElement, List<int> valueInLevel, int add)
    {
    }



    protected void AddValueHasTubeInLevel(MDLevel data, List<int> valueInLevel, int add)
    {
    }



    protected void AddValueNotTubeInLevel(LevelElement levelElement, MDLevel data)
    {
    }


    public MDLevel GetLevel(int value)
    {
        Debug.LogError("1"+  baseLevel == null);
        Debug.LogError("2" + baseLevel.baseLevels == null);
        Debug.LogError("3" + baseLevel.baseLevels.Count  );
        return baseLevel.baseLevels[value];
    }
}