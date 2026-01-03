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

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Place()
    {
        base.Place();

        _canAttack = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_canAttack) return;
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;
        if (PlayerManager.Instance.EnergyPoints < RunCost) return;

        // 可能要攻击
        Check();
    }

    /// <summary>
    /// 检测敌机
    /// </summary>
    protected virtual void Check()
    {
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
    }
    
    /// <summary>
    /// 上膛
    /// </summary>
    protected void SetCanAttack() => _canAttack = true;
    
}
