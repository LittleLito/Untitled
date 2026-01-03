using System.Collections.Generic;
using UnityEngine;

public class CattailBullet : MonoBehaviour
{
    public float Speed => 4;
    public int Damage => 5;
    // 是否还在运行
    private bool _alive;
    // 组件
    private Transform _target;
    
    public void Init(Vector3 pos)
    {
        transform.position = pos;
        transform.rotation = Quaternion.identity;

        var list = new List<EnemyBase>(EnemyManager.Instance.Enemies);
        list.Sort((base1, base2) => (int) (base1.transform.position.y - base2.transform.position.y) * 100);
        _target = list[0].transform;
        
        _alive = true;
    }

    private void Update()
    {
        if (!_alive) return;
        if (!GameConfig.BulletAvailableRect.Contains(transform.position)) Recycle();
        // 目标仍存活
        if (!_target.gameObject.activeInHierarchy && EnemyManager.Instance.Enemies.Count > 0)
        {
            var list = new List<EnemyBase>(EnemyManager.Instance.Enemies);
            list.Sort((base1, base2) => (int) (base1.transform.position.y - base2.transform.position.y) * 100);
            _target = list[0].transform;
        }

        // 追击
        if (EnemyManager.Instance.Enemies.Count > 0)
        {
            var angle = Vector3.SignedAngle(transform.up, _target.position - transform.position, Vector3.forward);
            transform.Rotate(new Vector3(0, 0, Mathf.Clamp(angle * Speed * Time.deltaTime, -3, 3)));
        }
        
        transform.position += Speed * Time.deltaTime * transform.up;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_alive) return;
        
        if (!other.CompareTag("Enemy")) return;
        other.GetComponent<IHitable>().Hit(Damage, this, false);
        _alive = false;
        Recycle();
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
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.CattailBullet, gameObject);

    }

}
