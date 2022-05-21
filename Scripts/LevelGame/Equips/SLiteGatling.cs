using UnityEngine;

public class SLiteGatling : GatlingBase, IMoonEnergyEquip
{
    public override int Cost => 0;
    public override float CD => 15;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.sLiteGatling;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.sLiteGatlingBullet;
    protected override float _attackCD => 1f;
    private SpriteRenderer _gunfire;

    protected override void FindComponent()
    {
        base.FindComponent();
        _gunfire = transform.Find("Gunfire").GetComponent<SpriteRenderer>();
        _gunfire.enabled = false;
    }
    
    protected override void Check()
    {
        // 检测射击范围内是否存在敌机
        var hit = Physics2D.Raycast((Vector2) transform.position + MuzzleOffset, transform.up,
            5, LayerMask.GetMask("Enemy"));
        if (hit.collider == null) return;
        
        Shoot();
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
