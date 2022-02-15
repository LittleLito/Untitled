using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static int TargetChapterNum;
    public static int TargetLevelNum;

    private static List<LevelInfo> _levelInfos;

    public static void InitLevelInfos()
    {
        var jsonStr = Resources.Load<TextAsset>("LevelInfo");
        var allLevelsInfo = JsonUtility.FromJson<AllLevelsInfo>(jsonStr.text);
        _levelInfos = allLevelsInfo.Levels;
    }

    public static LevelInfo GetLevelInfo()
    {
        return _levelInfos.Find(info =>
            info.Num == (TargetChapterNum - 1) * 10 + TargetLevelNum);

    }
    public static LevelInfo GetLevelInfo(int levelNum)
    {
        return _levelInfos.Find(info => info.Num == levelNum);
    }

    public static LevelInfo GetLevelInfo(int chapterNum, int levelNum)
    {
        return _levelInfos.Find(info =>
            info.Num == (chapterNum - 1) * 10 + levelNum);
    }
}
