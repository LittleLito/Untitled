using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        _spriteRenderer.sprite = GameManager.Instance.GameConfig.BulletImg;
        transform.position = pos;
        _alive = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (_alive)
        {
            if (transform.position.y < 6)
            {
                transform.Translate(Vector3.up * (Speed * Time.deltaTime * 0.5f));
            }
            else
            {
                Recycle();
            }
        }
    }

    /// <summary>
    /// 子弹接触其他物体
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        // 敌机
        if (col.gameObject.CompareTag("Enemy"))
        {
            // 击中爆炸图片
            _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.BulletBoom;
            
            // 击中敌机扣血 
            col.gameObject.GetComponent<EnemyBase>().Hit(Damage, false);

            // 不再可用
            _alive = false;
            Invoke(nameof(Recycle), 0.24f);
        }
    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    private void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.Bullet, gameObject);

    }
    
}
