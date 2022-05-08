using UnityEngine;

public class FrozenBullet20mm : BulletBase
{
    public override float Speed => 12;
    public override int Damage => 6;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.FrozenBullet;

    protected override void Explode(EnemyBase e)
    {
        _animator.Play("FrozenBulletBoom", 0, 0f);
        base.Explode(e);
        e.StatusEffectController.AddStatusEffect(StatusEffectType.Frozen, 1.25f, 3);
    }
}
