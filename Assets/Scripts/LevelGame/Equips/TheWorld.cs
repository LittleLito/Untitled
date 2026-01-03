using System;
using Chronos;
using UnityEngine;

public class TheWorld : MissileBase
{
    public override int Cost => 0;
    public override float CD => 5;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.TheWorld; 
    //public override bool IsSkill => true;
    public override float Speed => 0;
    public override float Damage => 0;
    protected override float _explosionScale => 0;
    protected override float _explosionRadius => 0;
    protected override GameObject _prefeb => GameManager.Instance.GameConfig.TheWorld;

    public override void Launch(Vector3 target)
    {
        //var a = Math.Abs(Timekeeper.instance.Clock("Root").localTimeScale - 1) < 0.1 ? 0 : 1;
        Timekeeper.instance.Clock("Root").LerpTimeScale(0, 3f);
        Invoke(nameof(Resume), 8);
    }

    private void Resume()
    {
        Timekeeper.instance.Clock("Root").LerpTimeScale(1, 3f);
    }
}
