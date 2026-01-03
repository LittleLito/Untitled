using UnityEngine;

/// <summary>
/// 装备基类
/// </summary>
public abstract class EquipBase : MonoBehaviour
{
    // 花费
    public abstract int Cost { get; }

    // CD
    public abstract float CD { get; }

    // 生命值
    protected float _health;
    public virtual float Health { get; set; }
    public virtual float MaxHealth => 50;

    // 运行时花费
    public virtual int RunCost => 0;

    // 系列
    public abstract EquipFamily Family { get; }

    // 种类
    public abstract EquipType Type { get; }

    // 装备图片
    public Sprite EquipImg => GetComponent<SpriteRenderer>().sprite;

    // 渲染器
    protected SpriteRenderer _spriteRenderer;

    /// <summary>
    /// 查找相关组件
    /// </summary>
    protected virtual void FindComponent()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 创建时初始化
    /// </summary>
    public void Create(bool inGrid, Vector3 pos)
    {
        FindComponent();
        transform.position = pos;
        
        // 不许动
        if (inGrid) // 如果是网格透明指示器
        {
            // 排序图层为-1
            _spriteRenderer.sortingOrder = -1;

            _spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            // 排序图层为0
            _spriteRenderer.sortingOrder = 0;

            _spriteRenderer.color = new Color(1, 1, 1, 1);

        }
    }

    /// <summary>
    /// 放置时初始化
    /// </summary>
    public virtual void Place()
    {
        Health = MaxHealth;

        PlayerManager.Instance.EnergyPoints -= Cost;
    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    public virtual void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(EquipManager.Instance.GetEquipByType(Type), gameObject);
    }
}