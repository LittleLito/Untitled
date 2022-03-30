using UnityEngine;

public class Boss305mmBullet : MonoBehaviour
{
    public readonly int Damage = 305;
    public readonly float Speed = 15f;
    private bool _alive;

    public void Init(Vector3 pos, Quaternion rot)
    {
        transform.rotation = rot;
        transform.localPosition = pos;
        transform.SetParent(null);
        tag = "ItalianGunBullet";
        _alive = true;
    }
    private void Update()
    {
        if (!_alive) return;

        transform.position += Speed * Time.deltaTime * -transform.up;
        if (transform.position.y < -6.7f)
        {
            Recycle();
        }
    }

    public void Explode()
    {
        tag = "Nothing";
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
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.Boss305mmBullet, gameObject);

    }
}
