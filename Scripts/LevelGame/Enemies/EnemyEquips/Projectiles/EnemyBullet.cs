using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // 速度
    public float Speed;

    // 伤害
    public int Damage;

    // 是否还在运行
    private bool _alive;

    // 组件
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Start is called before the first frame update
    public void Init(Vector3 pos)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = GameManager.Instance.GameConfig.EnemyBulletImg;
        transform.position = pos;
        _alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_alive) return;

        if (transform.position.y > -8)
        {
            transform.Translate(Vector3.down * (Speed * Time.deltaTime * 0.5f));
        }
        else
        {
            Recycle();
        }
    }

    /// <summary>
    /// 子弹爆炸
    /// </summary>
    public void Explode()
    {
        // 击中爆炸图片
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.EnemyBulletBoom;

        // 击中玩家扣血 
        PlayerManager.Instance.Health -= Damage;

        // 不再可用
        _alive = false;
        Invoke(nameof(Recycle), 0.5f);
    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    private void Recycle()
    {
        // 取消全部延迟调用
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.EnemyBullet, gameObject);
    }
}