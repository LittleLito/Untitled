using System;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType
{
    Health,
    Speed,
    Damage,
    AttackSpeed
}

public class AttributeModifier
{
    public AttributeType AttributeType;
    public float Value;

    public AttributeModifier(AttributeType attributeType, float value)
    {
        AttributeType = attributeType;
        Value = value;
    }
}

public class AttributeModifierManager
{
    public readonly List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();

    public AttributeModifierManager()
    {
        foreach (AttributeType type in Enum.GetValues(typeof(AttributeType)))
        {
            AttributeModifiers.Add(new AttributeModifier(type, 1f));
        }
    }

    public void AddModifier(AttributeType type, float value)
    {
        AttributeModifiers.Find(modifier => modifier.AttributeType == type).Value *= value;
    }

    public AttributeModifier GetModifier(AttributeType type)
    {
        return AttributeModifiers.Find(modifier => modifier.AttributeType == type);
    }

    public void RemoveAttributeModification(AttributeType type)
    {
        AttributeModifiers.Find(modifier => modifier.AttributeType == type).Value = 1;
    }

    public void Clear()
    {
        foreach (var modifier in AttributeModifiers)
        {
            modifier.Value = 1;
        }
    }
}