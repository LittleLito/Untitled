using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static int TargetChapterNum = 1;
    public static int TargetLevelNum = 1;

    private static List<LevelInfo> _levelInfos;
    public static List<LevelInfo> LevelInfos
    {
        get
        {
            // 如果不为空，则返回
            if (_levelInfos is { }) return _levelInfos;
            // 如果为空，则重新获取
            var jsonStr = Resources.Load<TextAsset>("LevelInfo");
            _levelInfos = JsonUtility.FromJson<AllLevelsInfo>(jsonStr.text).Levels;
            return _levelInfos;
        }
    }

    private static AlmanacDataOperator _almanacDataOperator;
    public static AlmanacDataOperator AlmanacDataOperator
    {
        get
        {
            // 如果不为空，则返回
            if (_almanacDataOperator is { }) return _almanacDataOperator;
            // 如果为空，则重新获取
            var jsonStr = Resources.Load<TextAsset>("AlmanacData");
            _almanacDataOperator = JsonUtility.FromJson<AlmanacDataOperator>(jsonStr.text);
            return _almanacDataOperator;
        }
    }


    public static LevelInfo GetLevelInfo()
    {
        return LevelInfos.Find(info =>
            info.Num == (TargetChapterNum - 1) * 10 + TargetLevelNum);
    }
    public static LevelInfo GetLevelInfo(int levelNum)
    {
        return LevelInfos.Find(info => info.Num == levelNum);
    }
    public static LevelInfo GetLevelInfo(int chapterNum, int levelNum)
    {
        return LevelInfos.Find(info =>
            info.Num == (chapterNum - 1) * 10 + levelNum);
    }
}