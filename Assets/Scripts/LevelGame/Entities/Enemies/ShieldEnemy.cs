using UnityEngine;

public class ShieldEnemy : EnemyBase
{
    public override float MaxHealth => 200;
    protected override float _speedRange => Random.Range(2f, 2.2f);
    public override int WEIGHT => 40;
    public override int LEVEL => 3;
    public override EnemyType Type => EnemyType.ShieldEnemy;
    public Sprite shieldEnemy2;
    public Sprite shieldEnemy3;
    protected override Sprite DamagedImgNo2 => shieldEnemy2;
    protected override Sprite DamagedImgNo3 => shieldEnemy3;
    protected override float _explosionScale => 2f;
    

    public override void Init(Vector3 pos)
    {
        base.Init(pos);

        PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.EnemyShield, transform)
            .GetComponent<EnemyShield>().Init(new Vector3(0, -0.59f, 0));
    }

    protected override void Explode()
    {
        transform.DetachChildren();
        base.Explode();
    }
}
