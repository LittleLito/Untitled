using UnityEngine;


public abstract class BulletBase : MonoBehaviour
{
    // 速度
    public abstract float Speed { get; }
    // 伤害
    public abstract int Damage { get; }
    // 子弹prefab
    protected abstract GameObject _prefab { get; }
    // 子弹爆炸动画
    protected abstract RuntimeAnimatorController _bulletBoom { get; }
    // 是否还在运行
    protected bool _alive;
    // 组件
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    
    // Start is called before the first frame update
    public virtual void Init(Vector3 pos)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = _prefab.GetComponent<SpriteRenderer>().sprite;
        transform.position = pos;
        _alive = true;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!_alive) return;
        
        if (transform.position.y < 6)
        {
            transform.Translate(Vector3.up * (Speed * Time.deltaTime * 0.5f));
        }
        else
        {
            Recycle();
        }
    }

    /// <summary>
    /// 子弹接触其他物体
    /// </summary>
    /// <param name="col"></param>
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        // 敌机
        if (col.gameObject.CompareTag("Enemy"))
        {
            // 击中爆炸图片
            _animator.runtimeAnimatorController = _bulletBoom;
            
            var e = col.gameObject.GetComponent<EnemyBase>();
            // 子弹其他效果
            BoomEffect(e);
            // 击中敌机扣血 
            e.Hit(Damage, false);
            
            // 不再可用
            _alive = false;
            Invoke(nameof(Recycle), 0.5f);
        }
    }

    protected virtual void BoomEffect(EnemyBase e)
    {
        
    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    protected virtual void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(_prefab, gameObject);

    }
}