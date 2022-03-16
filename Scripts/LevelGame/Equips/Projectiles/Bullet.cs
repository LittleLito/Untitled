using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BulletBase
{
    public override float Speed => 12;
    public override int Damage => 6;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.Bullet;
    protected override RuntimeAnimatorController _bulletBoom => GameManager.Instance.GameConfig.BulletBoom;
}
