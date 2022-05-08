using UnityEngine;

public class IMFrozenRocket : GatlingBase
{
    public override int Cost => 5000;
    public override int RunCost => 5;
    public override float CD => 50;
    public override EquipFamily Family => EquipFamily.Frozen;
    public override EquipType Type => EquipType.IMFrozenRocket;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.IMFrozenRocketBullet;
    protected override Vector2 MuzzleOffset => new Vector2(0, 0.066f);
    protected override float _attackCD => 1.4f;
    private Animator _gunfireAnim;

    protected override void FindComponent()
    {
        base.FindComponent();
        _gunfireAnim = transform.Find("Gunfire").GetComponent<Animator>();
    }

    protected override void GunFireEffect()
    {
        _gunfireAnim.Play("GMHeavyRocketGunfire", 0, 0f);
    }
}
