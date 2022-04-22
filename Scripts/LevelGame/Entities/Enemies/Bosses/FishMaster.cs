using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FishMasterState
{
    StateNull,
    StateFull,
    StateHalf
}
public class FishMaster : BossBase
{
    public override BossType BossType => BossType.FishMaster;
    public override float MaxHealth => 20000;
    private FishMasterState _fishMasterState;
    public FishMasterState FishMasterState
    {
        set
        {
            if (_fishMasterState.Equals(value)) return;

            _fishMasterState = value;
            switch (value)
            {
                case FishMasterState.StateFull:
                    for (var i = 1; i <= 5; i++)
                    {
                        transform.Find($"SGatling{i}").GetComponent<EnemySGatling>().Init();
                    }
                    break;
                case FishMasterState.StateHalf:
                    for (var i = 6; i <= 10; i++)
                    {
                        transform.Find($"SGatling{i}").GetComponent<EnemySGatling>().Init();
                    }
                    KillerCanAttack = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
    protected override float _speedRange => Random.Range(0.5f, 4f) * AttributeModifierManager.GetModifier(AttributeType.Speed).Value;
    protected override Sprite DamagedImgNo2 => GameManager.Instance.GameConfig.FishMaster2;
    protected override Sprite DamagedImgNo3 => GameManager.Instance.GameConfig.FishMaster3;
    protected override Sprite DamagedImgNo4 => GameManager.Instance.GameConfig.FishMaster4;
    protected override float _explosionScale => 7f;
    public override Color BossWarningColor => Color.red;

    // 区域内随机移动
    private bool CanMove
    {
        set
        {
            if (value)
            {
                transform.DOMove(new Vector3(Random.Range(-4.9f, 6.25f), Random.Range(3.14f, 5.06f), 0), _speedRange * 0.3f)
                    .SetSpeedBased().OnComplete(() => Invoke(nameof(SetCanMoveTrue), Random.Range(0f, 5f)));
            }
            else
            {              
                CancelInvoke(nameof(SetCanMoveTrue));
            }
        }
    }

    // Killer巡航导弹攻击
    private bool KillerCanAttack
    {
        set
        {
            if (value)
            {
                // 左右随机
                var x = Random.value < 0.5f ? -1 : 1;
                PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.KillerModelL, null)
                    .GetComponent<KillerBase>().Init(transform.position + new Vector3(x * 1.573f, -0.105f, 0));
                Invoke(nameof(SetKillerCanAttackTrue), Random.Range(25f, 35f));
                // 随机附赠一枚挂载的导弹
                var a = Random.value;
                if (a < 0.5f)
                {
                    Invoke(nameof(TryLaunchAnotherEquippedKiller), Random.Range(8f, 15f));
                }
            }
            else
            {
                CancelInvoke(nameof(SetKillerCanAttackTrue));
            }
        }
    }

    private void SetCanMoveTrue() => CanMove = true;
    private void SetKillerCanAttackTrue() => KillerCanAttack = true;
    // 尝试发射另一枚killer
    private void TryLaunchAnotherEquippedKiller()
    {
        Transform killer;
        do
        {
            // 随机选择一枚
            var a = new[] { 1, 2, 3, 4, 5, 6 }[Random.Range(0, 5)];
            // 是否还挂着
            killer = transform.Find($"KillerModelL{a}");
        } while (killer is null); // 直至随机找到一枚挂着的
        
        killer.SetParent(null);
        killer.GetComponent<KillerBase>().Init(killer.position);
    }

    public override void Init(Vector3 pos)
    {
        base.Init(pos);

        CanMove = false;
        KillerCanAttack = false;
        FishMasterState = FishMasterState.StateFull;

        // 十秒后执行另一些初始化
        Invoke(nameof(LateInits), 10);
    }
    // 另一些初始化
    private void LateInits() 
    {
        transform.Find("FishMaster2x305mmGun1").GetComponent<FishMaster2x305mmGun>().CanAttack = true;
        transform.Find("FishMaster2x305mmGun2").GetComponent<FishMaster2x305mmGun>().CanAttack = true;

        CanMove = true;
    }

    protected override void Update()
    {
        if (Health > 0 && LevelManager.Instance.LevelState == LevelState.Boss)
        {
        }
        base.Update();
    }

    protected override void CheckState(float before, float after)
    {
        if (before > MaxHealth * 1 / 2 && after <= MaxHealth * 1 / 2)
        {
            FishMasterState = FishMasterState.StateHalf;
        }
    }

    protected override void StopAllActivities()
    {
        transform.DOKill();
        CanMove = false;
        KillerCanAttack = false;
        base.StopAllActivities();
    }
}