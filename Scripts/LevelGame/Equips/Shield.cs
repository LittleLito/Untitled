using UnityEngine;

public class Shield : MissileBase
{
    public override int Cost => 750;
    public override float CD => 30;
    public override int MaxHealth => 250;
    public override int Health
    {
        get => _health;
        set
        {
            _health = value > 0 ? value : 0;

            if (_health <= 0)
            {
                Recycle();
            }
        }
    }
    public override EquipFamily Family => EquipFamily.Defence;
    public override EquipType Type => EquipType.Shield;
    public override float Speed => 6f;
    public override float Damage => 0;
    protected override float _explosionScale => 0;
    protected override float _explosionRadius => 0;

    public override Color GetColor()
    {
        return Color.green;
    }
    
    public override void Launch(Vector3 target)
    {
        // 花费能量
        PlayerManager.Instance.EnergyPoints -= Cost;
        
        target.z = 0;
        _target = target;
        
        // 查找组件
        base.FindComponent();
        _animator = GetComponent<Animator>();
        // 初始化
        GetComponent<BoxCollider2D>().enabled = false;
        transform.localScale = Vector3.one;
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = EquipManager.Instance.GetEquipByType(Type).GetComponent<SpriteRenderer>().sprite;
        // 调整位置
        transform.position = PlayerManager.Instance.transform.position;
        Tools.YLookAt(transform, target);

        _flying = true;

    }

    /// <summary>
    /// 与子弹碰撞时
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        print(Health);
        switch (other.tag)
        {
            // 撞到敌机子弹
            case "EnemyBullet":
                other.GetComponent<EnemyBullet>().Explode();
                Health -= other.GetComponent<EnemyBullet>().Damage;

                print(Health);
                LevelManager.Instance.Stats.IncreaseStat(StatType.Absorbed, 6);

                break;

        }
    }

    /// <summary>
    /// 盾牌展开
    /// </summary>
    protected override void Explode()
    {
        _flying = false;
        
        // 盾牌展开动画
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.ShieldOpen;
        
        // 生命初始化
        Health = MaxHealth;
        // 碰撞体启用
        GetComponent<BoxCollider2D>().enabled = true;
    }
}