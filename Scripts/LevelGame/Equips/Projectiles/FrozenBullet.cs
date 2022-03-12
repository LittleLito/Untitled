using UnityEngine;

public class FrozenBullet : BulletBase
{
    public override float Speed => 12;
    public override int Damage => 6;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.FrozenBullet;
    protected override RuntimeAnimatorController _bulletBoom => GameManager.Instance.GameConfig.FrozenBulletBoom;

    protected override void BoomEffect(EnemyBase e)
    {
        e.StatusEffectController.AddStatusEffect(StatusEffectType.Frozen, 1.25f, 3);
    }
}
