using UnityEngine;

public abstract class GatlingBase : EquipBase
{
    protected abstract GameObject Bullet { get; }
    protected abstract float _attackCD { get; }
    // 可以攻击与否
    protected bool _canAttack;
    // 枪口位置偏移
    protected virtual Vector2 MuzzleOffset => new Vector2(0, 0.1738f);
    // 枪口火焰
    protected SpriteRenderer _gunfire;

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Place()
    {
        base.Place();

        _canAttack = true;
    }

    // Start is called before the first frame update
    protected override void FindComponent()
    {
        base.FindComponent();
        
        _gunfire = transform.Find("Gunfire").GetComponent<SpriteRenderer>();
        _gunfire.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // 可能要攻击
        Check();
    }

    /// <summary>
    /// 检测敌机
    /// </summary>
    protected virtual void Check()
    {
        if (!_canAttack) return;
        if (LevelManager.Instance.LevelState != LevelState.InGame) return;
        if (PlayerManager.Instance.EnergyPoints < RunCost) return;

        // 检测射击范围内是否存在敌机
        var hit = Physics2D.Raycast((Vector2) transform.position + MuzzleOffset, transform.up,
            5.4f - transform.position.y, LayerMask.GetMask("Enemy"));
        if (hit.collider == null) return;
        
        Shoot();
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    protected virtual void Shoot()
    {
        // 发射
        var bullet = PoolManager.Instance.GetGameObj(Bullet, null).GetComponent<BulletBase>();
        bullet.Init(transform.position + (Vector3) MuzzleOffset);
        PlayerManager.Instance.EnergyPoints -= RunCost;
        
        GunFireEffect();
        
        // 进入发射CD
        _canAttack = false;
        Invoke(nameof(SetCanAttack), _attackCD);

    }

    /// <summary>
    /// 枪口火焰效果
    /// </summary>
    protected virtual void GunFireEffect()
    {
        _gunfire.enabled = true;
        Invoke(nameof(SetGunFire), 0.1f);
    }
    
    /// <summary>
    /// 上膛
    /// </summary>
    protected void SetCanAttack() => _canAttack = true;

    /// <summary>
    /// 枪口熄火
    /// </summary>
    private void SetGunFire() => _gunfire.enabled = false;
    
}
