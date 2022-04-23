using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    public BossBase Boss;
    
    private void Awake()
    {
        Instance = this;
    }

    public void InitLevelInfo()
    {
        if (LevelManager.Instance.LevelInfo.IsBoss) {
            // 通过关卡配置信息决定Boss
            Boss = GetBossByType((BossType)LevelManager.Instance.LevelInfo.Boss).GetComponent<BossBase>();
        }    
    }
    
    /// <summary>
    /// 通过类型获取Boss的预制体
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetBossByType(BossType type)
    {
        return type switch
        {
            BossType.FishMaster => GameManager.Instance.GameConfig.FishMaster,
            _ => null
        };
    }

}

public enum BossType
{
    FishMaster = 1
}