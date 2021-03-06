using System;
using System.Collections.Generic;

public enum AttributeType
{
    Health,
    Speed,
    Damage,
    AttackSpeed
}

public class AttributeModifier
{
    public readonly AttributeType AttributeType;
    public float Value;

    public AttributeModifier(AttributeType attributeType, float value)
    {
        AttributeType = attributeType;
        Value = value;
    }
}

public class AttributeModifierManager
{
    private readonly List<AttributeModifier> _attributeModifiers = new List<AttributeModifier>();

    public AttributeModifierManager()
    {
        foreach (AttributeType type in Enum.GetValues(typeof(AttributeType)))
        {
            _attributeModifiers.Add(new AttributeModifier(type, 1f));
        }
    }
    
    /// <summary>
    /// 获取某一属性的修改器
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AttributeModifier GetModifier(AttributeType type)
    {
        return _attributeModifiers.Find(modifier => modifier.AttributeType == type);
    }

    /// <summary>
    /// 将某一属性修改器的值归为1
    /// </summary>
    /// <param name="type"></param>
    public void RemoveAttributeModification(AttributeType type)
    {
        _attributeModifiers.Find(modifier => modifier.AttributeType == type).Value = 1;
    }

    /// <summary>
    /// 将所有属性修改器的值归为1
    /// </summary>
    public void Clear()
    {
        foreach (var modifier in _attributeModifiers)
        {
            modifier.Value = 1;
        }
    }
}