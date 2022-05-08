using UnityEngine;

public class GatlingBullet10mm : BulletBase
{
    public override float Speed => 10f;

    public override int Damage => 3;

    protected override GameObject _prefab => throw new System.NotImplementedException();
}
