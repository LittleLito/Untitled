using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : EnemyBase
{
    public override float MaxHealth => 1000;
    public override float Speed => Random.Range(2.8f, 3.2f);
    public override int WEIGHT => 40;
    public override int LEVEL => 1;
    public override EnemyType Type => EnemyType.NormalEnemy;
    protected override Sprite DamagedImgNo1 => GameManager.Instance.GameConfig.NormalEnemy1;
    protected override Sprite DamagedImgNo2 => GameManager.Instance.GameConfig.NormalEnemy2;
    protected override Sprite DamagedImgNo3 => GameManager.Instance.GameConfig.NormalEnemy3;
    protected override float _explosionScale => 0.8f;
}
