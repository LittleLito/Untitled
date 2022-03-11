using System;
using System.Linq;
using UnityEngine;

public class GMHeavyRocketBullet : BulletBase
{
    public override float Speed => 11f;
    public override int Damage => 30;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.GMHeavyRocketBullet;
    protected override RuntimeAnimatorController _bulletBoom => null;

    private TrailRenderer _trail;

    public override void Init(Vector3 pos)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _prefab.GetComponent<SpriteRenderer>().sprite;
        _trail = GetComponent<TrailRenderer>();
        _trail.emitting = true;
        transform.position = pos;
        _alive = true;
        _spriteRenderer.enabled = true;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        // 敌机
        if (col.gameObject.CompareTag("Enemy"))
        {
            // 击中敌机扣血 
            col.gameObject.GetComponent<EnemyBase>().Hit(Damage, false);
            // 子弹其他效果
            
            // 不再可用
            _alive = false;
            _spriteRenderer.enabled = false;
            Invoke(nameof(Recycle), 0.6f);
            
            GetComponent<ParticleSystem>().Play();
            // 造成伤害
            foreach (var e in EnemyManager.Instance.Enemies.Where(e => Vector2.Distance(e.transform.position, transform.position) < 2))
            {
                e.Hit(7, false);
            }

        }

    }

    protected override void Recycle()
    {
        _trail.emitting = false;
        base.Recycle();
    }
}
