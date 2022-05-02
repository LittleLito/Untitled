using UnityEngine;

public class Cattail : GatlingBase
{
    public override int Cost => 2250;
    public override int RunCost => 2;
    public override float CD => 30;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.Cattail;
    protected override GameObject Bullet => GameManager.Instance.GameConfig.CattailBullet;
    protected override float _attackCD => 0.3f;
    protected override Vector2 MuzzleOffset => new Vector2(-0.004f, 0.15f);

    private Animator _gunfireAnim;

    public override void Place()
    {
        base.Place();

        _gunfireAnim = transform.Find("Gunfire").GetComponent<Animator>();
    }

    protected override void Check()
    {
        if (!_canAttack) return;
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;
        if (PlayerManager.Instance.EnergyPoints < RunCost) return;
        
        if (EnemyManager.Instance.Enemies.Count == 0) return;
        
        Shoot();
    }

    protected override void Shoot()
    {
        // 发射
        var bullet = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.CattailBullet, null)
            .GetComponent<CattailBullet>();
        bullet.Init(transform.position + (Vector3) MuzzleOffset);
        PlayerManager.Instance.EnergyPoints -= RunCost;
        
        GunFireEffect();
        
        // 进入发射CD
        _canAttack = false;
        Invoke(nameof(SetCanAttack), _attackCD);


    }

    protected override void GunFireEffect()
    {
        _gunfire.enabled = true;
        _gunfireAnim.Play("CattailGunfire", 0, 0f);
        
        Invoke(nameof(SetSpriteRendererEnabledFalse), 0.17f);
    }

    private void SetSpriteRendererEnabledFalse() => _gunfire.enabled = false;
}
