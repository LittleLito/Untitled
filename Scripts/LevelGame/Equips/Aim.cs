using UnityEngine;

public class Aim : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;

            _spriteRenderer.color = value;
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.Aim, gameObject);
    }
}
