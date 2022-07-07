using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    // 速度
    public abstract float Speed { get; }
    // 伤害
    public abstract int Damage { get; }
    // 子弹prefab
    protected abstract GameObject _prefab { get; }
    // 是否还在运行
    protected bool _alive;
    // 组件
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    
    public virtual void Init(Vector3 pos)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        transform.position = pos;
        _alive = true;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!_alive) return;
        
        if (GameConfig.BulletAvailableRect.Contains(transform.position))
        {
            Move();
        }
        else
        {
            Recycle();
        }
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.up * (Speed * Time.deltaTime * 0.5f));
    }

    /// <summary>
    /// 子弹接触其他物体
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_alive) return;
        
        switch (col.tag)
        {
            // 敌机
            case "Enemy":
                Explode(col.GetComponent<IHitable>());
                break;
        }
    }

    protected virtual void Explode(IHitable e)
    {
        // 击中敌机扣血 
        e.Hit(Damage, false);
            
        // 不再可用
        _alive = false;
    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    public virtual void Recycle()
    {
        _spriteRenderer.sprite = _prefab.GetComponent<SpriteRenderer>().sprite;

        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(_prefab, gameObject);

    }
}