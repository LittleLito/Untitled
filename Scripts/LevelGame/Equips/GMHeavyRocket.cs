using UnityEngine;

public class GMHeavyRocket : GatlingBase
{
    public override int Cost => 3000;
    public override int RunCost => 3;
    public override float CD => 7.5f;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.GMHeavyRocket;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.GMHeavyRocketBullet;
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
