using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
[CreateAssetMenu(fileName = "newMapData", menuName = "Data/Map Data")]
public class GameMapDataSo : ScriptableObject
{
    public int roomNumber;
    public int RewardDepth;
    public int bossDepth;
    public MapGeneratType generatType;
    public List<MapUnit> rooms;

    public MapUnit GetRandomMatchingMapUnit(Func<MapUnit, bool> condition)
    {
        var matches = rooms.Where(condition).ToList();

        if (matches.Count == 0)
            return null;

        var random = new System.Random();
        int index = random.Next(matches.Count);
        return matches[index];
    }

    public List<MapUnit> GetAllMatchingMapUnit(Func<MapUnit, bool> condition)
    {
        var matches = rooms.Where(condition).ToList();

        if (matches.Count == 0)
            return null;

        return matches;
    }
}

public enum MapGeneratType
{
    CapitalFields,
    CapitalRoad,
    CapitalRailWay,
    Capital,
    PlatinumPalaceOuter,//铂金宫外
    Sewer,//下水道
    Harbour,//海港
    PlatinumPalaceOuterFirst,
    PlatinumPalaceOuterSecond,
    Catacombs,//地下墓穴
    Ossuary,//藏骨堂
    DivineCity,//神眷之城
    Cathedral//大教堂
}