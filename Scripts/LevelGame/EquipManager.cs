using System;
using System.Collections;
using System.Collections.Generic;
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
            EquipType.GMHeavyRocket => GameManager.Instance.GameConfig.GMHeavyRocket,
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
}

public enum EquipType
{
    SGatling = 1,
    EnergyReactor = 2,
    CherryMissile = 3,
    DelayBomb = 4,
    Shield = 5,
    FrozenSGatling = 6,
    GMHeavyRocket
}

public enum EquipFamily
{
    Common,
    Enlighten,
    Doom,
    Defence,
    Frozen
}

public interface IOneTimeUseEquip
{
    Color GetColor();

    void Launch(Vector3 target);
}
