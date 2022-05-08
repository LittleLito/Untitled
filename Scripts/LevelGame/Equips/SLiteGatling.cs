using UnityEngine;

/*public class SLiteGatling : GatlingBase
{
    public override int Cost => 0;
    public override float CD => 15;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.SLiteGatling;
    protected override GameObject Bullet { get; }
    protected override float _attackCD => 1f;

    protected override void Check()
    {
        // 检测射击范围内是否存在敌机
        var hit = Physics2D.Raycast((Vector2) transform.position + MuzzleOffset, transform.up,
            5, LayerMask.GetMask("Enemy"));
        if (hit.collider == null) return;
        
        Shoot();
    }
}*/
