using System;
using DG.Tweening;
using UnityEngine;

public abstract class KillerBase : MonoBehaviour
{
    protected abstract GameObject KillerType { get; }
    protected abstract int Damage { get; }
    protected abstract float MaxSpeed { get; }
    protected abstract float MaxRotateAngle { get; }
    [NonSerialized]
    protected float Speed;
    protected abstract float _explosionScale { get; }
    protected abstract float _explosionRadius { get; }
    protected abstract float _duration { get; }
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private Transform _target;
    private bool _alive;
    
    public void Init(Vector3 pos)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0, 0, 180);
        transform.localScale = Vector3.one;
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.sprite = KillerType.GetComponent<SpriteRenderer>().sprite;
        tag = "Killer";

        _target = PlayerManager.Instance.transform;
        _alive = true;
        // 开始加速
        Speed = 0;
        DOTween.To(() => Speed, value => Speed = value, MaxSpeed, 2f).SetSpeedBased().SetEase(Ease.OutSine);
        
        // 一段时间后燃料不足自动损毁
        Invoke(nameof(Explode), _duration);
    }

    private void Update()
    {
        if (!_alive) return;
        
        if (PlayerManager.Instance != null)
        {
            var angle = Vector3.SignedAngle(transform.up, _target.position - transform.position, Vector3.forward);
            transform.Rotate(new Vector3(0, 0, Mathf.Clamp(angle, -MaxRotateAngle, MaxRotateAngle)));
        }
            
        transform.position += Speed * Time.deltaTime * transform.up;
            
        if (!Rect.MinMaxRect(-14f, -11f, 14f, 11f).Contains(transform.position))
        {
            Recycle();
        }
    }

    /// <summary>
    /// 爆炸产生伤害
    /// </summary>
    public void Explode()
    {
        _alive = false;
        tag = "Nothing";
        
        // 击中爆炸图片
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.ExplosionAnim;
        transform.localScale = new Vector3(_explosionScale, _explosionScale, 0);
        
        Invoke(nameof(Recycle), 0.917f);

        var cols = new Collider2D[100];
        Physics2D.OverlapCircleNonAlloc(transform.position, _explosionRadius, cols);
        foreach (var col in cols)
        {
            if (col is null) break;
            
            switch (col.tag)
            {
                case "Player":
                    PlayerManager.Instance.Health -= Damage;
                    Camera.main.DOShakePosition(Damage / 1000f, Damage / 1000f);
                    break;
                case "Shield":
                    col.GetComponent<Shield>().Hit(Damage);
                    break;
            }
        }

    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    private void Recycle()
    {
        // 取消延迟调用
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(KillerType, gameObject);
    }
    
}
