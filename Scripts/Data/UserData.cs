using System;
using System.Collections.Generic;

[Serializable]
public struct UserLevelData
{
    public int LevelNum;
    public bool Passed;
    public float MaxScore;
}

[Serializable]
public class UserData
{
    public int CoinNum;
    public int ReachedLevelNum;
    public List<UserLevelData> UserLevelDataList = new List<UserLevelData>();

}
