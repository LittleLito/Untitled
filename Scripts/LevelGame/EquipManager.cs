using System;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 通过类型获取装备的预制体
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetEquipByType(EquipType type)
    {
        return type switch
        {
            EquipType.SGatling => GameManager.Instance.GameConfig.SGatling,
            EquipType.EnergyReactor => GameManager.Instance.GameConfig.EnergyReactor,
            EquipType.CherryMissile => GameManager.Instance.GameConfig.CherryMissile,
            EquipType.DelayBomb => GameManager.Instance.GameConfig.DelayBomb,
            EquipType.Shield => GameManager.Instance.GameConfig.Shield,
            EquipType.FrozenSGatling => GameManager.Instance.GameConfig.FrozenSGatling,
            EquipType.SPGatling => GameManager.Instance.GameConfig.SPGatling,
            EquipType.Cattail => GameManager.Instance.GameConfig.Cattail,
            EquipType.sLiteGatling => GameManager.Instance.GameConfig.sLiteGatling,
            EquipType.MoonEnergyReactor => GameManager.Instance.GameConfig.MoonEnergyReactor,
            EquipType.GMHeavyRocket => GameManager.Instance.GameConfig.GMHeavyRocket,
            EquipType.IMFrozenRocket => GameManager.Instance.GameConfig.IMFrozenRocket,
            _ => null
        };
    }

    /// <summary>
    /// 通过系列获取装备卡片背景
    /// </summary>
    /// <param name="family"></param>
    /// <returns></returns>
    public Sprite GetCardImgByFamily(EquipFamily family)
    {
        return family switch
        {
            EquipFamily.Common => GameManager.Instance.GameConfig.CardCommon,
            EquipFamily.Enlighten => GameManager.Instance.GameConfig.CardEnlighten,
            EquipFamily.Doom => GameManager.Instance.GameConfig.CardDoom,
            EquipFamily.Defence => GameManager.Instance.GameConfig.CardDefence,
            EquipFamily.Frozen => GameManager.Instance.GameConfig.CardFrozen,
            _ => null
        };
    }

    /// <summary>
    /// 通过类型获取弹射物的预制体
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public GameObject GetProjectileByType(ProjectileType type)
    {
        return type switch
        {
            ProjectileType.Bullet => GameManager.Instance.GameConfig.Bullet,
            ProjectileType.FrozenBullet => GameManager.Instance.GameConfig.FrozenBullet,
            ProjectileType.CattailBullet => GameManager.Instance.GameConfig.CattailBullet,
            ProjectileType.GMHeavyRocket => GameManager.Instance.GameConfig.GMHeavyRocketBullet,
            ProjectileType.IMFrozenRocket => GameManager.Instance.GameConfig.IMFrozenRocketBullet,
            ProjectileType.sLiteGatlingBullet => GameManager.Instance.GameConfig.sLiteGatlingBullet,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        }; 
    }
}

public enum EquipType
{
    SGatling = 1,
    EnergyReactor = 2,
    CherryMissile = 3,
    DelayBomb = 4,
    Shield = 5,
    FrozenSGatling = 6,
    SPGatling = 7,
    Cattail = 8,
    sLiteGatling = 9,
    MoonEnergyReactor = 10,
    GMHeavyRocket = 15,
    IMFrozenRocket = 16
}

public enum EquipFamily
{
    Common,
    Enlighten,
    Doom,
    Defence,
    Frozen
}

public enum ProjectileType
{
    Bullet = 1,
    FrozenBullet = 2,
    CattailBullet = 3,
    GMHeavyRocket = 4,
    IMFrozenRocket = 5,
    sLiteGatlingBullet = 6
}

public interface IOneTimeUseEquip
{
    Color GetColor();

    void Launch(Vector3 target);
}

public interface IMoonEnergyEquip
{
    
}
