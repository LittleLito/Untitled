using UnityEngine;

public class IMFrozenRocketBullet : BulletBase
{
    public override float Speed => 11f;
    public override int Damage => 25;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.IMFrozenRocketBullet;

    private TrailRenderer _trail;

    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        _trail = GetComponent<TrailRenderer>();
        _trail.emitting = true;
        _spriteRenderer.enabled = true;
    }

    protected override void Explode(IHitable e)
    {
        base.Explode(e);
        
        _spriteRenderer.enabled = false;
        GetComponent<ParticleSystem>().Play();
        
        // 造成伤害
        var cols = new Collider2D[100];
        Physics2D.OverlapCircleNonAlloc(transform.position, 2, cols, LayerMask.GetMask("Enemy"));
        foreach (var col in cols)
        {
            if (col is null) break;
            var ihit = col.GetComponent<IHitable>();
            ihit.Hit(10, false);
            if (ihit is EnemyBase enemyBase){
                enemyBase.StatusEffectController.AddStatusEffect(StatusEffectType.Frozen, 1.428f, 5);
            }        
        }

    }

    private void OnParticleSystemStopped()
    {
        _trail.emitting = false;
        Recycle();
    }

}
