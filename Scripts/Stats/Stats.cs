using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats
{
    private List<Stat> _stats = new List<Stat>();
    private double _startTime;

    public void InitStats()
    {
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            _stats.Add(new Stat(type, 0f));
        }
    }

    public void StartTiming()
    {
        _startTime = Time.timeAsDouble;
    }

    public double GetTime()
    {
        return Time.timeAsDouble - _startTime;
    }

    /// <summary>
    /// 增加一项数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="increment"></param>
    public void IncreaseStat(StatType type, float increment)
    {
        GetStatWithType(type).Value += increment;
    }

    /// <summary>
    /// 通过类别获取数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Stat GetStatWithType(StatType type)
    {
        return _stats.Find(stat => stat.Type == type);
    }
}

/// <summary>
/// 数据种类
/// </summary>
public enum StatType
{
    Killed,
    Damage,
    Absorbed
}

/// <summary>
/// 数据类
/// </summary>
public class Stat
{
    public StatType Type;
    public float Value;

    public Stat(StatType type, float value)
    {
        Type = type;
        Value = value;
    }
}
