using UnityEngine;

public class FrozenSGatling : GatlingBase
{
    public override int Cost => 1750;
    public override int RunCost => 1;
    public override float CD => 7.5f;
    public override EquipFamily Family => EquipFamily.Frozen;
    public override EquipType Type => EquipType.FrozenSGatling;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.FrozenBullet;
    protected override float _attackCD => 0.7f;

    public override void Place()
    {
        base.Place();
        GetComponent<ParticleSystem>().Play();
    }

    public override void Recycle()
    {
        GetComponent<ParticleSystem>().Stop();
        base.Recycle();
    }
}