using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySGatling : MonoBehaviour
{
    private readonly float _attackCD = 0.7f;

    // 可以攻击与否
    private bool _canAttack;

    // 枪口位置偏移
    private readonly Vector2 _muzzleOffset = new Vector2(0, -0.1738f);

    // 枪口火焰
    private SpriteRenderer _gunfire;

    // Start is called before the first frame update
    private void Start()
    {
        _gunfire = transform.Find("Gunfire").GetComponent<SpriteRenderer>();
        _gunfire.enabled = false;
        _canAttack = true;
    }

    // Update is called once per frame
    private void Update()
    {
        
        // 可能要攻击
        Shoot();
    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    private void Shoot()
    {
        if (!_canAttack) return;
        if (LevelManager.Instance.LevelState != LevelState.InGame) return;
        // 发射
        var bullet = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.EnemyBullet, null)
            .GetComponent<EnemyBullet>();
        bullet.Init(transform.position + (Vector3) _muzzleOffset);

        // 枪口火焰效果
        _gunfire.enabled = true;
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
        _gunfire.enabled = false;
    }
}