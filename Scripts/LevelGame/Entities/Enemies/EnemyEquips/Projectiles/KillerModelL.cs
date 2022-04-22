using UnityEngine;

public class KillerModelL : KillerBase
{
    protected override GameObject KillerType => GameManager.Instance.GameConfig.KillerModelL;
    protected override int Damage => 250;
    protected override float MaxSpeed => 5f;
    protected override float MaxRotateAngle => 1.5f;
    protected override float _explosionScale => 6.28f;
    protected override float _explosionRadius => 2f;
    protected override float _duration => 15f;
}
