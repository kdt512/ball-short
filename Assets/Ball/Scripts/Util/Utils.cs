using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Utils
{
    public static Random ran = new Random();


    public static void Shuffle<T>(this List<T> t)
    {
        bool isEnd = false;
        int i = t.Count;
        while (!isEnd)
        {
            while (i > 1)
            {
                i--;
                int value = ran.Next(i + 1);
                T count = t[value];
                t[value] = t[i];
                t[i] = count;
            }

            isEnd = true;
        }
    }


    public static void ShuffleDuplicate(List<int> t)
    {
        int index = 0;
        for (int i = 0; i < t.Count; i += 4)
        {
            for (int j = 1; j <= 4; j++)
            {
                if (t[j - 1] == t[j])
                {
                    index++;
                }
            }

            if (index >= 3)
            {
                Shuffle<int>(t);
            }
        }
    }


    public static float GetDurationMove(Vector2 start, Vector2 end)
    {
        return Vector2.Distance(start, end) * Constans.DURATION_MOVE_PER_UNIT;
    }
}