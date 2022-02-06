using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public virtual int Health { get; set; }

    public virtual int MaxHealth => 300;

    // 运行时花费
    public virtual int RunCost => 0;

    // 系列
    public abstract EquipFamily Family { get; }

    // 种类
    public abstract EquipType Type { get; }

    // 装备图片
    public virtual Sprite EquipImg => GetComponent<SpriteRenderer>().sprite;

    // 渲染器
    protected SpriteRenderer SpriteRenderer;

    /// <summary>
    /// 查找相关组件
    /// </summary>
    protected void FindComponent()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
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
            SpriteRenderer.sortingOrder = -1;

            SpriteRenderer.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            // 排序图层为0
            SpriteRenderer.sortingOrder = 0;

            SpriteRenderer.color = new Color(1, 1, 1, 1);

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
    public void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(EquipManager.Instance.GetEquipByType(Type), gameObject);
    }
}