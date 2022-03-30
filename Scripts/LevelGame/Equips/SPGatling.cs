using UnityEngine;

public class SPGatling : GatlingBase
{ 
    protected override GameObject Bullet => GameManager.Instance.GameConfig.Bullet;
    protected override float _attackCD => 0.7f;
    public override int Cost => 2000;
    public override int RunCost => 2;
    public override float CD => 7.5f;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.SPGatling;

    private Transform SGatling1;
    private Transform SGatling2;
    private SpriteRenderer _gunfire1;
    private SpriteRenderer _gunfire2;

    protected override void FindComponent()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        SGatling1 = transform.Find("SGatling1");
        _gunfire1 = transform.Find("SGatling1/Gunfire").GetComponent<SpriteRenderer>();
        _gunfire1.enabled = false;
        SGatling2 = transform.Find("SGatling2");
        _gunfire2 = transform.Find("SGatling2/Gunfire").GetComponent<SpriteRenderer>();
        _gunfire2.enabled = false;
    }
    
    protected override void Check()
    {
        if (!_canAttack) return;
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;
        if (PlayerManager.Instance.EnergyPoints < RunCost) return;

        // 检测射击范围内是否存在敌机
        var hit1 = Physics2D.Raycast((Vector2) SGatling1.position + MuzzleOffset, SGatling1.up,
            5.4f - SGatling1.position.y, LayerMask.GetMask("Enemy"));
        var hit2 = Physics2D.Raycast((Vector2) SGatling2.position + MuzzleOffset, SGatling2.up,
            5.4f - SGatling2.position.y, LayerMask.GetMask("Enemy"));

        if (hit1.collider is null && hit2.collider is null) return;
        
        Shoot();

    }

    protected override void Shoot()
    {
        ShootBulletOnce();
        Invoke(nameof(ShootBulletOnce), 0.12f);
        PlayerManager.Instance.EnergyPoints -= RunCost;

        // 进入发射CD
        _canAttack = false;
        Invoke(nameof(SetCanAttack), _attackCD);

    }

    /// <summary>
    /// 发射一次子弹
    /// </summary>
    private void ShootBulletOnce()
    {
        // 发射
        var bullet1 = PoolManager.Instance.GetGameObj(Bullet, null).GetComponent<BulletBase>();
        bullet1.Init(SGatling1.position + (Vector3) MuzzleOffset);
        var bullet2 = PoolManager.Instance.GetGameObj(Bullet, null).GetComponent<BulletBase>();
        bullet2.Init(SGatling2.position + (Vector3) MuzzleOffset);
        
        GunFireEffect();
    }

    protected override void GunFireEffect()
    {
        _gunfire1.enabled = true;
        _gunfire2.enabled = true;
        Invoke(nameof(SetGunFire), 0.1f);

    }

    protected override void SetGunFire()
    {
        _gunfire1.enabled = false;
        _gunfire2.enabled = false;
    }
}
