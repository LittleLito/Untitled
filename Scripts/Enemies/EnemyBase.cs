using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public abstract class EnemyBase : MonoBehaviour
{
    // 动画器
    private Animator _animator;

    // 渲染器
    private SpriteRenderer _spriteRenderer;

    // 生命值
    public abstract float MaxHealth { get; }
    private float _health;

    public float Health
    {
        get => _health;
        set
        {
            if (value.Equals(_health)) return;
            _health = value <= 0 ? 0 : value;

            // 检查图片
            _spriteRenderer.sprite = GetDamagedImg();

            // 更新敌机系统
            _enemyHealthUpdateAction?.Invoke();

            // 爆炸
            if (_health <= 0)
            {
                Explode();
            }
        }
    }

    private UnityAction _enemyHealthUpdateAction;

    // 速度
    public abstract float Speed { get; }

    // 权重
    public abstract int WEIGHT { get; }

    // 等级
    public abstract int LEVEL { get; }

    // 种类
    public abstract EnemyType Type { get; }

    // 破损1图片
    protected abstract Sprite DamagedImgNo2 { get; }

    // 破损2图片
    protected abstract Sprite DamagedImgNo3 { get; }

    // 爆炸大小
    protected abstract float _explosionScale { get; }

    /// <summary>
    /// 初始化位置等
    /// </summary>
    public virtual void Init(Vector3 pos)
    {
        // 查找组件
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // 初始状态
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = GetDamagedImg();
        Health = MaxHealth;
        Order();
        transform.localScale = Vector3.one;
        transform.position = pos;
        gameObject.tag = "Enemy";
        gameObject.layer = 10;

        // 我们是同志了
        EnemyManager.Instance.AddEnemy(this);
        // 添加监听
        _enemyHealthUpdateAction += EnemyManager.Instance.UpdateCurrentHealthSum;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Health <= 0) return;

        // 在到家前移动
        if (transform.position.y > PlayerManager.Instance.DeadlineY)
        {
            Move();
        }
        // 到家了
        else
        {
            // 改变关卡状态
            if (LevelManager.Instance.LevelState == LevelState.InGame)
            {
                LevelManager.Instance.LevelState = LevelState.Over;
                
                // 移动摄像机
                Camera.main.transform.DOMoveY(-4.88f, 3.5f);
            }
            
            // 继续飞行
            if (transform.position.y > -11.5f)
            {
                transform.Translate(0, -Time.deltaTime, 0);
            }
            // 进家，飞行结束
            else
            {
                LevelManager.Instance.LevelState = LevelState.Conclusion;
            }
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    protected virtual void Move()
    {
        if (LevelManager.Instance.LevelState == LevelState.InGame)
        {
            transform.Translate(0, -Speed * Time.deltaTime * 0.1f, 0);
        }
    }

    /// <summary>
    /// 决定排列顺序，避免渲染重叠闪烁
    /// </summary>
    private void Order()
    {
        var sortingOrder = (int) (transform.position.x * 100);
        _spriteRenderer.sortingOrder = sortingOrder;
    }

    /// <summary>
    /// 根据现有血量决定显示图片
    /// </summary>
    /// <returns></returns>
    protected virtual Sprite GetDamagedImg()
    {
        if (Health > MaxHealth * 2 / 3)
        {
            return EnemyManager.Instance.GetEnemyByType(Type).GetComponent<SpriteRenderer>().sprite;
        }

        if (Health > MaxHealth * 1 / 3)
        {
            return DamagedImgNo2;
        }
        else
        {
            return DamagedImgNo3;
        }
    }

    /// <summary>
    /// 受击
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="fromPlayer"></param>
    public void Hit(float damage, bool fromPlayer)
    {
        Health -= damage;

        if (fromPlayer) return;
        LevelManager.Instance.Stats.IncreaseStat(StatType.Damage, damage);
    }

    /// <summary>
    /// 生命值小于等于0后执行的操作
    /// </summary>
    protected virtual void Explode()
    {
        Invoke(nameof(Recycle), 0.88f);
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.Explosion;
        transform.localScale = new Vector3(_explosionScale, _explosionScale, 0);
        gameObject.tag = "Nothing";
        gameObject.layer = 11;

        LevelManager.Instance.Stats.IncreaseStat(StatType.Killed, 1f);
    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    public virtual void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 从列表中删除
        EnemyManager.Instance.RemoveEnemy(this);

        // 回库
        PoolManager.Instance.PushGameObj(EnemyManager.Instance.GetEnemyByType(Type), gameObject);
    }
}