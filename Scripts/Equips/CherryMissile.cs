using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryMissile : MissileBase, IOneTimeUseEquip
{
    public override int Cost => 1500;
    public override float CD => 0f;
    public override EquipFamily Family => EquipFamily.Doom;
    public override EquipType Type => EquipType.CherryMissile;
    public override float Speed => 5f;
    public override float Damage => 500;
    protected override float _explosionScale => 5.5f;
    protected override float _explosionRadius => 2f;
}
