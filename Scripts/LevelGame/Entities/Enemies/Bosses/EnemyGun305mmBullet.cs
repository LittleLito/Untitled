using UnityEngine;

public class EnemyGun305mmBullet : EnemyGunBulletBase
{
    public override int Damage => 305;
    public override float Speed => 15f;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.EnemyGun305mmBullet;
}
