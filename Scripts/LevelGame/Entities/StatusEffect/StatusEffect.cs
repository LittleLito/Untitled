using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public enum StatusEffectType
{
    Frozen
}

public class StatusEffect
{
    public StatusEffectType StatusEffectType;
    public float Value;
    public float Duration;

    // 装有此状态效果的列表
    private StatusEffectController _controller;
    // 计时器
    private Timer _timer;

    public StatusEffect(StatusEffectType statusEffectType, float value, float duration, StatusEffectController controller)
    {
        StatusEffectType = statusEffectType;
        Value = value;
        Duration = duration;
        _controller = controller;

        _timer = new Timer(100);
        _timer.Elapsed += (sender, args) => DecreaseDuration();
    }

    // 计算时间
    private void DecreaseDuration()
    {
        Duration -= 0.1f;
        if (Duration <= 0)
        {
            Clear();
        }
    }

    public void StartTimer()
    {
        _timer.Start();
    }

    public void Clear()
    {
        _timer.Stop();
        _timer.Dispose();

        _controller.StatusEffects.Remove(this);
        _controller.UpdateStates();
    }
    
}

public class StatusEffectController
{
    private readonly MonoBehaviour _statusEffectTarget;
    public List<StatusEffect> StatusEffects = new List<StatusEffect>();

    public StatusEffectController(MonoBehaviour e)
    {
        _statusEffectTarget = e;
    }
    /// <summary>
    /// 添加新状态效果
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="percentage"></param>
    /// <param name="duration"></param>
    public void AddStatusEffect(StatusEffectType stateType, float percentage, float duration)
    {
        // 如果状态效果已经存在，则叠加
        if (StatusEffects.Exists(effect => effect.StatusEffectType == stateType))
        {
            var statusEffect = StatusEffects.Find(effect => effect.StatusEffectType == stateType);
            statusEffect.Value *= percentage;
            statusEffect.Duration += duration;
        }
        // 如果还未拥有，则添加
        else
        {
            var statusEffect = new StatusEffect(stateType, percentage, duration, this);
            StatusEffects.Add(statusEffect);
            statusEffect.StartTimer();
        }
        
        UpdateStates();
        
    }

    /// <summary>
    /// 清除所有状态效果
    /// </summary>
    public void ClearStatusEffects()
    {
        while (StatusEffects.Count > 0)
        {
            StatusEffects[0].Clear();
            StatusEffects.RemoveAt(0);
        }
    }

    /// <summary>
    /// 更新目标及其子部件状态
    /// </summary>
    public void UpdateStates()
    {
        foreach (var effect in StatusEffects)
        {
            ((IStatusEffectHandler) _statusEffectTarget).HandleStatusEffect(effect);
            for (var i = 0; i < _statusEffectTarget.transform.childCount; i++)
            {
                var c = _statusEffectTarget.transform.GetChild(i).GetComponent<MonoBehaviour>();
                if (c is IStatusEffectHandler handler)
                {
                    handler.HandleStatusEffect(effect);
                }
            }
        }
    }
    
}

public interface IStatusEffectHandler
{
    void HandleStatusEffect(StatusEffect effect);
}