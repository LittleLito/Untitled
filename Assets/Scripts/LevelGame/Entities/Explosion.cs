using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void Init(Vector3 pos, Vector3 scale)
    {
        // 位置和缩放
        transform.position = pos;
        transform.localScale = scale;
        // 重新播放
        GetComponent<Animator>().Play("Explosion", 0, 0f);
        
        // 结束回收
        Invoke(nameof(Recycle), 0.917f);
    }

    private void Recycle()
    {
        CancelInvoke();
        
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.Explosion, gameObject);
    }
}
