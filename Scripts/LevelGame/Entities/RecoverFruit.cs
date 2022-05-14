using UnityEngine;

public class RecoverFruit : MonoBehaviour
{
    public const int RecoverPoint = 100;
        
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    public void Init()
    {
        // 查找组件
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        // 组件初始化
        transform.position = new Vector3(Random.Range(-7.5f, 8.5f), Random.Range(-6.5f, 3f), 0);
        _spriteRenderer.sprite = GameManager.Instance.GameConfig.RecoverFruit.GetComponent<SpriteRenderer>().sprite;
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.RecoverFruit.GetComponent<Animator>()
            .runtimeAnimatorController;

        // 显示
        gameObject.SetActive(true);
        
        Invoke(nameof(SetAnimatorControllerShake), 0.66f);
    }
    private void SetAnimatorControllerShake()
    {
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.RecoverFruitShake;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        tag = "RecoverFruit";
        
        // 若不捡，则消失
        Invoke(nameof(Recycle), 20);
    }


    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    public void Recycle()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        tag = "Nothing";

        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.RecoverFruit, gameObject);
    }

}
