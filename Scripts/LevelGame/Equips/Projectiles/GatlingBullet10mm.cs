using UnityEngine;

public class GatlingBullet10mm : BulletBase
{
    public override float Speed => 10f;
    public override int Damage => 3;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.sLiteGatlingBullet;

    private float _shootY;
    
    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        _shootY = pos.y;
    }

    protected override void Update()
    {
        if (!_alive) return;
        
        if (Rect.MinMaxRect(-11.77f, -7f, 11.77f, _shootY + 5).Contains(transform.position))
        {
            Move();
        }
        else
        {
            Recycle();
        }
    }

    protected override void Explode(EnemyBase e)
    {
        base.Explode(e);
        Recycle();
    }
}
