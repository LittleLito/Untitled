using UnityEngine;

public class FrozenSGatling : GatlingBase
{
    public override int Cost => 0;
    public override int RunCost => 1;
    public override float CD => 7.5f;
    public override EquipFamily Family => EquipFamily.Frozen;
    public override EquipType Type => EquipType.FrozenSGatling;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.FrozenBullet;
    protected override float _attackCD => 0.7f;
}