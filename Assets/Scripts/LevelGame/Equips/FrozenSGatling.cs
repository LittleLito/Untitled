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
    private SpriteRenderer _gunfire;

    protected override void FindComponent()
    {
        base.FindComponent();
        _gunfire = transform.Find("Gunfire").GetComponent<SpriteRenderer>();
        _gunfire.enabled = false;
    }

    public override void Place()
    {
        base.Place();
        GetComponent<ParticleSystem>().Play();
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


    public override void Recycle()
    {
        GetComponent<ParticleSystem>().Stop();
        base.Recycle();
    }
}