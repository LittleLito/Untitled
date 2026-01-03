using System;
using System.Collections.Generic;

[Serializable]
public struct LevelInfo
{
    public int Num;
	public bool IsNight;
    public List<int> Enemies;
    public bool IsBoss;
    public int Boss;
    public int MaxWaveNum;
    public int PassScore;
}

[Serializable]
public struct AllLevelsInfo
{
    public List<LevelInfo> Levels;
}