using UnityEngine;

public class GatlingBullet40mm : BulletBase
{
    public override float Speed => 12;
    public override int Damage => 6;
    protected override GameObject _prefab => GameManager.Instance.GameConfig.Bullet;

    protected override void Explode(IHitable e)
    {        
        // 击中爆炸图片
        _animator.Play("BulletBoom", 0, 0f);
        base.Explode(e);
    }
}
