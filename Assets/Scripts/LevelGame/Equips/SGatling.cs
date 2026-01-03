using UnityEngine;

public class SGatling : GatlingBase
{
    public override int Cost => 1000;
    public override int RunCost => 1;
    public override float CD => 7.5f;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.SGatling;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.Bullet;
    protected override float _attackCD => 0.7f;
    private SpriteRenderer _gunfire;

    protected override void FindComponent()
    {
        base.FindComponent();
        _gunfire = transform.Find("Gunfire").GetComponent<SpriteRenderer>();
        _gunfire.enabled = false;

    }

    protected override void GunFireEffect()
    {
        _gunfire.enabled = true;
        Invoke(nameof(SetGunFire), 0.1f);
    }
    
    /// <summary>
    /// 枪口熄火
    /// </summary>
    private void SetGunFire() => _gunfire.enabled = false;

}