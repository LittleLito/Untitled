using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishMaster2x305mmGun : MonoBehaviour
{
    public const float RotateSpeed = 1f;
    public readonly Vector3[] MuzzleOffsets = {new Vector3(-0.095f, -0.82f, 0), new Vector3(0.095f, -0.82f, 0)};
    public readonly float[] CD = {8f, 10f};
    [NonSerialized]
    public bool CanAttack = false;
    private bool _inCD;
    
    /// <summary>
    /// 瞄准玩家，发射子弹
    /// </summary>
    private void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.Boss) return;
        // 玩家仍存活
        if (PlayerManager.Instance.Health <= 0) return;
        
        // 对准玩家
        var angle = Vector3.SignedAngle(-transform.up, PlayerManager.Instance.transform.position - transform.position,
            Vector3.forward);
        transform.Rotate(new Vector3(0, 0, Mathf.Clamp(angle * RotateSpeed * Time.deltaTime, -1, 1)));

        // 准备发射
        if (!CanAttack || _inCD) return;
        // 发射两颗子弹
        var bullet1 = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Boss305mmBullet, transform)
            .GetComponent<Boss305mmBullet>();
        bullet1.Init(MuzzleOffsets[0], transform.rotation);
        var bullet2 = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Boss305mmBullet, transform)
            .GetComponent<Boss305mmBullet>();
        bullet2.Init(MuzzleOffsets[1], transform.rotation);

        // 进入CD
        _inCD = true;
        Invoke(nameof(SetInCDFalse), Random.Range(CD[0], CD[1]));
    }
    
    private void SetInCDFalse() => _inCD = false;
}
