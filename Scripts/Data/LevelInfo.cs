using System;
using System.Collections.Generic;

[Serializable]
public struct LevelInfo
{
    public int Num;
    public List<int> Enemies;
    public int MaxWaveNum;
    public int PassScore;
}

[Serializable]
public struct AllLevelsInfo
{
    public List<LevelInfo> Levels;
}