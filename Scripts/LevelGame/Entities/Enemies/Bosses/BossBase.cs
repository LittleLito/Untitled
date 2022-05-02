using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BossBase : EnemyBase
{
    public override float MaxHealth { get; }
    protected override float _speedRange { get; }
    public override float Health
    {
        get => _health;
        protected set
        {
            if (value.Equals(_health)) return;
            
            // 检查图片
            CheckDamagedImg(_health, value);
            // 检查状态
            CheckState(_health, value);

            _health = value <= 0 ? 0 : value;
            
            UIManager.Instance.bossBarPanel.UpdateBossBarInfo(_health);

            if (_health <= 0)
            {
                Explode();
            }
            
        }
    }

    public override int WEIGHT { get; }
    public override int LEVEL { get; }
    public override EnemyType Type { get; }
    protected override Sprite DamagedImgNo2 { get; }
    protected override Sprite DamagedImgNo3 { get; }
    protected virtual Sprite DamagedImgNo4 { get; }
    protected override float _explosionScale { get; }
    public abstract BossType BossType { get; }
    // 出场Warning的图片颜色
    public abstract Color BossWarningColor { get; }

    private PolygonCollider2D _collider;

    public override void Init(Vector3 pos)
    {
        // 查找组件
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();

        // 初始状态
        if (StatusEffectController is null)
        {
            StatusEffectController = new StatusEffectController(this);
        }
        else
        {
            StatusEffectController.ClearStatusEffects();
        }

        AttributeModifierManager = new AttributeModifierManager();
        Speed = _speedRange;
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = BossManager.Instance.GetBossByType(BossType).GetComponent<SpriteRenderer>().sprite;
        Health = MaxHealth;
        transform.localScale = Vector3.one;
        gameObject.tag = "Enemy";
        gameObject.layer = 10;

        // 我们是同志了
        EnemyManager.Instance.AddEnemy(this);

    }

    protected override void Update()
    {
        if (Health <= 0 || PlayerManager.Instance != null) return;
        
        StopAllActivities();

        // 在到家前移动
        if (transform.position.y > PlayerManager.DeadlineY)
        {
            transform.Translate(0, -_speedRange * Time.deltaTime * 0.5f, 0);
        }
        // 到家了
        else
        {
            // 改变关卡状态
            if (LevelManager.Instance.LevelState == LevelState.Boss)
            {
                LevelManager.Instance.LevelState = LevelState.Over;
                
                // 移动摄像机
                Camera.main.transform.DOMoveY(-4.88f, 3.5f);
            }
            
            // 继续飞行
            if (transform.position.y > -13.5f)
            {
                transform.Translate(0, -_speedRange * Time.deltaTime * 0.5f, 0);
            }
            // 进家，飞行结束
            else
            {
                LevelManager.Instance.LevelState = LevelState.Conclusion;
            }
        }

    }
    
    /// <summary>
    /// 根据现有血量决定显示图片
    /// </summary>
    /// <returns></returns>
    private void CheckDamagedImg(float before, float after)
    {
        if (before > MaxHealth * 3 / 4 && after <= MaxHealth * 3 / 4)
        {            
            _spriteRenderer.sprite = DamagedImgNo2;
        }
        else if (before > MaxHealth * 1 / 2 && after <= MaxHealth * 1 / 2)
        {
            _spriteRenderer.sprite = DamagedImgNo3;
        }
        else if (before > MaxHealth * 1 / 4 && after <= MaxHealth * 1 / 4)
        {
            _spriteRenderer.sprite = DamagedImgNo4;
        }

    }

    /// <summary>
    /// 根据现有血量决定行为状态
    /// </summary>
    /// <param name="before"></param>
    /// <param name="after"></param>
    protected abstract void CheckState(float before, float after);

    public override void HandleStatusEffect(StatusEffect effect)
    {
        // 子对象处理
        for (var i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i).GetComponent<MonoBehaviour>();
            if (c is IStatusEffectHandler handler)
            {
                handler.HandleStatusEffect(effect);
            }
        }

        AttributeModifierManager.Clear();

        _spriteRenderer.color = Color.white;
        
        if (effect is null) return;
        
        switch (effect.StatusEffectType)
        {
            case StatusEffectType.Frozen:
                // 减速
                AttributeModifierManager.GetModifier(AttributeType.Speed).Value = Math.Max(1 / effect.Value, 0.6f);
                // 改变颜色
                _spriteRenderer.DOColor(Color.cyan, 0.5f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }


    /// <summary>
    /// 停止活动
    /// </summary>
    protected virtual void StopAllActivities()
    {
        // 隐藏子对象
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 多次爆炸后总爆炸
    /// </summary>
    protected override void Explode()
    {
        gameObject.tag = "Nothing";
        gameObject.layer = 11;
        
        // boss血槽隐藏
        UIManager.Instance.bossBarPanel.SetVisible(false);
        // boss停止攻击和移动行为
        StopAllActivities();

        InvokeRepeating(nameof(SpawnExplosion), 0, 0.2f);
        Invoke(nameof(HugeExplosion), 5);
    }
    // 生成单个爆炸
    private void SpawnExplosion()
    {
        // 让爆炸产生于飞机上
        Vector3 v;
        do
        {
            v = new Vector3(Random.Range(-9f, 9f), Random.Range(-6.7f, 5.34967f), 0);
        } while (!_collider.OverlapPoint(v));

        var e = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Explosion, transform).GetComponent<Explosion>();
        e.Init(v, Random.Range(0f, _explosionScale / 2) * Vector3.one);
    }
    // 最后产生巨大爆炸
    private void HugeExplosion()
    {
        // 清除小碎爆炸
        CancelInvoke(nameof(SpawnExplosion));
        // 清除效果
        StatusEffectController.ClearStatusEffects();
        // 清除所有颜色渐变动画
        _spriteRenderer.DOKill();
        
        Invoke(nameof(Recycle), 0.917f);
        
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.ExplosionAnim;
        transform.localScale = new Vector3(_explosionScale, _explosionScale, 0);
        
        LevelManager.Instance.Stats.IncreaseStat(StatType.Killed, 1f);
    }

    /// <summary>
    /// boss无需回收直接销毁即可
    /// </summary>
    public override void Recycle()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
