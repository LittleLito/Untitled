using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class LevelInfo
{
    public int Num;
    public List<int> Enemies;
    public int MaxWaveNum;
}

[Serializable]
public class AllLevelsInfo
{
    public List<LevelInfo> Levels;
}