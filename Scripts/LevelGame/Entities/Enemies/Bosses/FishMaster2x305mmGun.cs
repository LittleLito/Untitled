using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class FishMaster2x305mmGun : MonoBehaviour
{
    public const float RotateSpeed = 1f;
    private readonly Vector3[] _muzzleOffsets = { new Vector3(-0.095f, -0.82f, 0), new Vector3(0.095f, -0.82f, 0) };
    public readonly float[] CD = { 15f, 20f };
    private float _scatter => Random.Range(-2f, 2f);
    [NonSerialized] public bool CanAttack = false;
    private bool _inCD;

    public Light2D L1;
    public Light2D L2;

    /// <summary>
    /// 瞄准玩家，发射子弹
    /// </summary>
    private void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.Boss) return;
        // 玩家仍存活
        if (PlayerManager.Instance == null) return;

        // 对准玩家
        var angle = Vector3.SignedAngle(-transform.up, PlayerManager.Instance.transform.position - transform.position,
            Vector3.forward);
        transform.Rotate(new Vector3(0, 0, Mathf.Clamp(angle * RotateSpeed * Time.deltaTime, -1, 1)));

        // 准备发射
        if (!CanAttack || _inCD) return;
        // 进入CD
        _inCD = true;
        Invoke(nameof(SetInCDFalse), Random.Range(CD[0], CD[1]));
        
        DOTween.To(() => L1.intensity, value =>
        {
            L1.intensity = value;
            L2.intensity = value;
        }, 30, 1).SetEase(Ease.InSine).OnComplete(Shoot);
    }

    private void Shoot()
    {
        // 亮度归零
        L1.intensity = 0;
        L2.intensity = 0;
        
        // 发射两颗子弹
        var bullet1 = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.EnemyGun305mmBullet, transform)
            .GetComponent<EnemyGunBulletBase>();
        bullet1.Init(_muzzleOffsets[0], transform.rotation);
        bullet1.transform.Rotate(bullet1.transform.forward, _scatter);
        var bullet2 = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.EnemyGun305mmBullet, transform)
            .GetComponent<EnemyGunBulletBase>();
        bullet2.Init(_muzzleOffsets[1], transform.rotation);
        bullet2.transform.Rotate(bullet2.transform.forward, _scatter);
    }

    private void SetInCDFalse() => _inCD = false;
}