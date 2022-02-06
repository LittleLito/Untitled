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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            _ => null
        };
    }
}

public enum EquipType
{
    SGatling = 1,
    EnergyReactor = 2
}

public enum EquipFamily
{
    Common,
    Enlighten
}
