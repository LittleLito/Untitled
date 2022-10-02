using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using VolumetricLines;

public class LaserPointer : EquipBase, IMoonEnergyEquip
{
    public override int Cost => 750;
    public override float CD => 7.5f;
    public override int RunCost => 1;
    public readonly float Damage = 2f;
    public override EquipFamily Family => EquipFamily.Common;
    public override EquipType Type => EquipType.LaserPointer;

    public VolumetricLineBehavior line;
    public Light2D light;
    public GameObject laserhitlight;

    protected bool _canAttack;
    private bool _attacking;
    private RaycastHit2D[] _targets;

    protected override void FindComponent()
    {
        base.FindComponent();
        line.LineWidth = 0;
    }

    public override void Place()
    {
        base.Place();
        _canAttack = true;
    }

    private void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame &&
            LevelManager.Instance.LevelState != LevelState.Boss) return;
        if (!_canAttack || PlayerManager.Instance.EnergyPoints < RunCost) return;

        // 可能要攻击
        // 检测射击范围内是否存在敌机
        var hit = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, 0.105f), transform.up,
            5.4f - transform.position.y, LayerMask.GetMask("Enemy"));
        if (hit.collider == null)
        {
            GunfireEffect(false);
            _attacking = false;
            return;
        }

        if (_attacking) return;
        GunfireEffect(true);
        _attacking = true;
        StartCoroutine(nameof(LaserAttack));
    }

    private void GunfireEffect(bool fire)
    {
        if (fire)
        {
            DOTween.To(() => line.LineWidth, value => line.LineWidth = value, 0.3f, 0.2f).SetEase(Ease.Linear);
            DOTween.To(() => light.intensity, value => light.intensity = value, 5f, 0.2f).SetEase(Ease.Linear);
        }
        else
        {
            DOTween.To(() => line.LineWidth, value => line.LineWidth = value, 0, 0.2f).SetEase(Ease.Linear);
            DOTween.To(() => light.intensity, value => light.intensity = value, 0, 0.2f).SetEase(Ease.Linear);
        }
    }

    /// <summary>
    /// 检测敌机进行攻击
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaserAttack()
    {
        // 获取敌机
        _targets = Physics2D.RaycastAll((Vector2)transform.position + new Vector2(0, 0.105f), transform.up,
            5.4f - transform.position.y, LayerMask.GetMask("Enemy"));
        // 伤害
        foreach (var hit in _targets)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                SpawnLight(hit.point);
                hit.collider.GetComponent<IHitable>().Hit(Damage, this, false);
            }
        }
            
        PlayerManager.Instance.EnergyPoints -= RunCost;
        if (PlayerManager.Instance.EnergyPoints < RunCost) _attacking = false;

        // 当攻击时
        while (_attacking)
        {
            yield return new WaitForSeconds(0.3f);
            
            // 获取敌机
            _targets = Physics2D.RaycastAll((Vector2)transform.position + new Vector2(0, 0.105f), transform.up,
                5.4f - transform.position.y, LayerMask.GetMask("Enemy"));
            // 伤害
            foreach (var hit in _targets)
            {
                SpawnLight(hit.point);
                hit.collider.GetComponent<IHitable>().Hit(Damage, this, false);
            }
            
            PlayerManager.Instance.EnergyPoints -= RunCost;
            if (PlayerManager.Instance.EnergyPoints < RunCost) break;
        }
    }

    private void SpawnLight(Vector3 pos)
    {
        // 激光命中闪光效果
        var hitlight = Instantiate(laserhitlight, pos, Quaternion.identity).GetComponent<Light2D>();
        Destroy(hitlight.gameObject, 0.4f);
        
    }
    
}