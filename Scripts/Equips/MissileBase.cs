using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MissileBase : EquipBase, IOneTimeUseEquip
{
    public override int Cost { get; }
    public override float CD { get; }
    public override EquipFamily Family { get; }
    public override EquipType Type { get; }
    public abstract float Speed { get; }
    public abstract float Damage { get; }
    protected abstract float _explosionScale { get; }
    
    protected abstract float _explosionRadius { get; }
    public virtual Color GetColor() => Color.red;

    protected bool flying;
    protected Vector3 _target;
    protected Animator _animator;
    protected virtual void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame) return;

        switch (flying)
        {
            // 没飞到，飞
            case true when Vector3.Distance(transform.position, _target) > 0.1f:
                transform.position += transform.up * (Speed * Time.deltaTime);
                break;
            // 飞到了，爆
            case true:
                Explode();
                break;
        }
    }

    public void Launch(Vector3 target)
    {
        target.z = 0;
        _target = target;
        
        // 查找组件
        base.FindComponent();
        _animator = GetComponent<Animator>();
        GetComponent<PolygonCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = false;
        // 初始化
        transform.localScale = Vector3.one;
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = EquipManager.Instance.GetEquipByType(Type).GetComponent<SpriteRenderer>().sprite;
        // 调整位置
        transform.position = PlayerManager.Instance.transform.position;
        Tools.AxisLookAt(transform, target);

        flying = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (flying)
        {
            Explode();
        }
    }

    protected void Explode()
    {
        flying = false;

        GetComponent<CircleCollider2D>().enabled = true;
        
        // 击中爆炸图片
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.Explosion;
        transform.localScale = new Vector3(_explosionScale, _explosionScale, 0);
        
        Invoke(nameof(Recycle), 0.917f);

        foreach (var e in EnemyManager.Instance.Enemies.Where(e => Vector2.Distance(e.transform.position, transform.position) < _explosionRadius))
        {
            print(e.Health);
            e.Hit(Damage, false);
            print(e.Health);
        }
    }
}
