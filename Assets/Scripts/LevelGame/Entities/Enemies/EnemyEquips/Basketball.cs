using UnityEngine;

public class Basketball : MonoBehaviour, IHitable
{
    private float MaxHealth = 500;
    private float _health;
    public float Health
    {
        get => _health;
        set
        {
            _health = value > 0 ? value : 0;
            
            if (_health > 0) return;
            
            Explode();
        }
    }
    
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private IKunEnemy _parent;

    public void Init(Vector3 pos, IKunEnemy parent)
    {
        gameObject.layer = 10;
        tag = "Enemy";
        Health = MaxHealth;
        animator.Play("New State");
        spriteRenderer.sprite = GameManager.Instance.GameConfig.Basketball.GetComponent<SpriteRenderer>().sprite;
        transform.localPosition = pos;
        _parent = parent;
    }

    private void Update()
    {
        transform.Rotate(0, 0, -1);
    }

    public void Hit(float damage, MonoBehaviour source, bool fromPlayer)
    {
        Health -= damage;

        if (fromPlayer) return;
        LevelManager.Instance.Stats.IncreaseStat(StatType.Damage, damage);
    }

    public void Explode()
    {
        gameObject.layer = 11;
        tag = "Nothing";
            
        _parent.State = 2; 
        animator.Play("BasketballBoom", 0, 0f);
    }
    
    private void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        Destroy(gameObject);
    }

}
