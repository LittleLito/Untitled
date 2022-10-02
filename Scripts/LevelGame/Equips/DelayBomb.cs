using UnityEngine;

public class DelayBomb : MissileBase
{
    public override int Cost => 250;
    public override float CD => 20;
    public override EquipFamily Family => EquipFamily.Doom;
    public override EquipType Type => EquipType.DelayBomb;
    public override float Speed => 6.0f;

    public override float Damage => charged ? 500 : 100;
    protected override float _explosionScale => charged ? 4.15f : 2.44f;
    protected override float _explosionRadius => charged ? 1.3f : 0.6f;
    protected override GameObject _prefeb => GameManager.Instance.GameConfig.DelayBomb;

    private bool charged;
    protected override void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;

        switch (_flying)
        {
            // 没飞到，飞
            case true when Vector3.Distance(transform.position, _target) > 0.5f:
                transform.position += transform.up * (Speed * Time.deltaTime);
                break;
            // 飞到了，进入充能状态
            case true:
                _flying = false;
                Invoke(nameof(SetChargedTrue), 8);
                break;
        }

    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        CancelInvoke();
        Explode();
    }

    /// <summary>
    /// 发射炸弹
    /// </summary>
    /// <param name="target"></param>
    public override void Launch(Vector3 target)
    {
        charged = false;
        base.Launch(target);
    }

    /// <summary>
    /// 充能完毕
    /// </summary>
    private void SetChargedTrue()
    {
        charged = true;
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.DelayBombAnim;
    }
}