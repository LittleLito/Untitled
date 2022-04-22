using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats
{
    private readonly List<Stat> _stats = new List<Stat>();
    private double _startTime;
    private double _pausedTime;
    private double _pausedTimeSum;

    /// <summary>
    /// 初始化所有数据
    /// </summary>
    public void InitStats()
    {
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            _stats.Add(new Stat(type, 0f));
        }
    }

    /// <summary>
    /// 开始计算游戏时间
    /// </summary>
    public void StartTiming()
    {
        _startTime = Time.timeAsDouble;
        _pausedTime = 0;
        _pausedTimeSum = 0;
    }

    public void PauseTiming() => _pausedTime = Time.timeAsDouble;
    public void RestartTiming() => _pausedTimeSum += Time.timeAsDouble - _pausedTime;

    /// <summary>
    /// 获取游戏时间
    /// </summary>
    /// <returns></returns>
    public double GetTime() => Time.timeAsDouble - _startTime - _pausedTimeSum;

    /// <summary>
    /// 增加一项数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="increment"></param>
    public void IncreaseStat(StatType type, float increment) => GetStatWithType(type).Value += increment;

    /// <summary>
    /// 通过类别获取数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Stat GetStatWithType(StatType type) => _stats.Find(stat => stat.Type == type);
}

/// <summary>
/// 数据种类
/// </summary>
public enum StatType
{
    Killed,
    Damage,
    Absorbed,
    Healed
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
