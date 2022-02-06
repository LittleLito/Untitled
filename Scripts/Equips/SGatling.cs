using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class SGatling : EquipBase
{
    public override int Cost => 1000;
    public override float CD => 0f;
    public sealed override int RunCost => 1;
    public override EquipType Type => EquipType.SGatling;
    public override EquipFamily Family => EquipFamily.Common;
    // 攻击间隔
    private float _attackCD = 0.5f;
    // 可以攻击与否
    private bool _canAttack = false;
    // 枪口位置偏移
    private Vector2 _muzzleOffset = new Vector2(0, 0.1738f);
    // 枪口火焰
    private SpriteRenderer _gunfire;

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Place()
    {
        base.Place();

        _canAttack = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gunfire = transform.Find("Gunfire").GetComponent<SpriteRenderer>();
        _gunfire.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        // 可能要攻击
        Shoot();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 发射子弹
    /// </summary>
    private void Shoot()
    {
        if (!_canAttack) return;
        if (PlayerManager.Instance.EnergyPoints < 1) return;

            // 检测射击范围内是否存在敌机
        var hit = Physics2D.Raycast((Vector2) transform.position + _muzzleOffset, transform.up, 5.5f - transform.position.y,
            LayerMask.GetMask("Enemy"));
        if (hit.collider == null) return;

        // 发射
        var bullet = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Bullet, null).GetComponent<Bullet>();
        bullet.Init(transform.position + (Vector3) _muzzleOffset);
        PlayerManager.Instance.EnergyPoints -= RunCost;
        
        // 枪口火焰效果
        _gunfire.sprite = GameManager.Instance.GameConfig.Gunfire;
        Invoke(nameof(SetGunFire), 0.1f);
        
        // 进入发射CD
        _canAttack = false;
        Invoke(nameof(SetCanAttack), _attackCD);
    }

    /// <summary>
    /// 上膛
    /// </summary>
    private void SetCanAttack()
    {
        _canAttack = true;
    }

    /// <summary>
    /// 枪口熄火
    /// </summary>
    private void SetGunFire()
    {
        _gunfire.sprite = null;
    }
}