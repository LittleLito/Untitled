using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class SGatling : GatlingBase
{
    public override int Cost => 1000;
    public override int RunCost => 1;
    public override float CD => 7.5f;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.SGatling;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.Bullet;
    protected override float _attackCD => 0.7f;
}