using UnityEngine;

public class NormalEnemy : EnemyBase
{
    public override float MaxHealth => 30;
    protected override float _speedRange => Random.Range(2.4f, 2.8f);
    public override int WEIGHT => 40;
    public override int LEVEL => 1;
    public override EnemyType Type => EnemyType.NormalEnemy;
    public Sprite normalEnemy2;
    public Sprite normalEnemy3;
    protected override Sprite DamagedImgNo2 => normalEnemy2;
    protected override Sprite DamagedImgNo3 => normalEnemy3;
    protected override float _explosionScale => 0.8f;
}
