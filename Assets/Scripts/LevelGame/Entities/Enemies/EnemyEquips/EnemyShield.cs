using UnityEngine;

public class EnemyShield : MonoBehaviour, IHitable
{
    private float MaxHealth = 300;
    private float _health;
    public float Health
    {
        get => _health;
        set
        {
            CheckDamagedImg(_health, value);

            _health = value > 0 ? value : 0;

            if (_health <= 0)
            {
                Recycle();
            }
        }
    }
    
    // 破损图片
    public Sprite damagedImgNo2;
    public Sprite damagedImgNo3;
    private Sprite DamagedImgNo2 => damagedImgNo2;
    private Sprite DamagedImgNo3 => damagedImgNo3;

    public SpriteRenderer _spriteRenderer;

    public void Init(Vector3 pos)
    {
        _spriteRenderer.sprite = GameManager.Instance.GameConfig.EnemyShield.GetComponent<SpriteRenderer>().sprite;
        Health = MaxHealth;
        transform.localPosition = pos;
        transform.localScale = Vector3.one;
    }
    public void Hit(float damage, MonoBehaviour source, bool fromPlayer)
    {
        Health -= damage;

        if (fromPlayer) return;
        LevelManager.Instance.Stats.IncreaseStat(StatType.Damage, damage);
    }
    private void CheckDamagedImg(float before, float after)
    {
        if (before >= MaxHealth * 2 / 3 && after <= MaxHealth * 2 / 3)
        {            
            _spriteRenderer.sprite = DamagedImgNo2;
        }
        else if (before >= MaxHealth * 1 / 3 && after <= MaxHealth * 1 / 3)
        {
            _spriteRenderer.sprite = DamagedImgNo3;
        }

    }
    private void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.EnemyShield, gameObject);

    }
}
