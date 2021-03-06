using UnityEngine;

/// <summary>
/// 导弹基类
/// </summary>
public abstract class MissileBase : EquipBase, IOneTimeUseEquip
{
    public abstract float Speed { get; }
    public abstract float Damage { get; }
    // 爆炸规模，用于显示爆炸动画
    protected abstract float _explosionScale { get; }
    // 爆炸半径，用于检测敌机
    protected abstract float _explosionRadius { get; }
    // 准星颜色
    public virtual Color GetColor() => Color.red;

    protected bool _flying;
    protected Vector3 _target;
    protected Animator _animator;
    protected virtual void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;

        switch (_flying)
        {
            // 没飞到，飞
            case true when Vector3.Distance(transform.position, _target) > 0.5f:
                transform.position += transform.up * (Speed * Time.deltaTime);
                break;
            // 飞到了，爆
            case true:
                Explode();
                break;
        }
    }

    /// <summary>
    /// 发射导弹，其实是初始化
    /// </summary>
    /// <param name="target"></param>
    public virtual void Launch(Vector3 target)
    {
        // 花费能量
        PlayerManager.Instance.EnergyPoints -= Cost;
        
        target.z = 0;
        _target = target;
        
        // 查找组件
        base.FindComponent();
        _animator = GetComponent<Animator>();
        // 初始化
        GetComponent<PolygonCollider2D>().enabled = true;
        transform.localScale = Vector3.one;
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = EquipManager.Instance.GetEquipByType(Type).GetComponent<SpriteRenderer>().sprite;
        // 调整位置
        transform.position = PlayerManager.Instance.transform.position;
        ToolFuncs.YLookAt(transform, target);

        _flying = true;

    }

    /// <summary>
    /// 碰撞器触发器，飞行途中检测有没有碰撞敌机，如有则就地爆炸
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (_flying)
        {
            Explode();
        }
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    protected virtual void Explode()
    {
        _flying = false;
        
        // 击中爆炸图片
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.ExplosionAnim;
        transform.localScale = new Vector3(_explosionScale, _explosionScale, 0);
        
        Invoke(nameof(Recycle), 0.917f);

        var cols = new Collider2D[100];
        Physics2D.OverlapCircleNonAlloc(transform.position, _explosionRadius, cols, LayerMask.GetMask("Enemy"));
        foreach (var col in cols)
        {
            if (col is null) break;
            col.GetComponent<IHitable>().Hit(Damage, false);
        }
    }
}
