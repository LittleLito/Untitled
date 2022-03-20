using UnityEngine;

public class GMHeavyRocket : GatlingBase
{
    public override int Cost => 3000;
    public override int RunCost => 5;
    public override float CD => 7.5f;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.GMHeavyRocket;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.GMHeavyRocketBullet;
    protected override Vector2 MuzzleOffset => new Vector2(0, 0.066f);
    protected override float _attackCD => 1.5f;

    protected override void GunFireEffect()
    {
        _gunfire.sprite = GameManager.Instance.GameConfig.GMRocketGunfire1;
        
        Invoke(nameof(SetSpriteRendererEnabledFalse), 0.33f);
    }

    private void SetSpriteRendererEnabledFalse() => _gunfire.enabled = false;
}
