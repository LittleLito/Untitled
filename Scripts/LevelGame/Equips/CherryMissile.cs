using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryMissile : MissileBase, IOneTimeUseEquip
{
    public override int Cost => 1500;
    public override float CD => 50f;
    public override EquipFamily Family => EquipFamily.Doom;
    public override EquipType Type => EquipType.CherryMissile;
    public override float Speed => 7f;
    public override float Damage => 500;
    protected override float _explosionScale => 7.5f;
    protected override float _explosionRadius => 2.5f;
}
