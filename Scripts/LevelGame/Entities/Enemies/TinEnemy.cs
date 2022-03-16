using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinEnemy : EnemyBase
{
    public override float MaxHealth => 300;
    protected override float _speedRange => Random.Range(2.4f, 2.8f);
    public override int WEIGHT => 30;
    public override int LEVEL => 4;
    public override EnemyType Type => EnemyType.TinEnemy;
    protected override Sprite DamagedImgNo2 => GameManager.Instance.GameConfig.TinEnemy2;
    protected override Sprite DamagedImgNo3 => GameManager.Instance.GameConfig.TinEnemy3;
    protected override float _explosionScale => 2.3f;

    private GameObject _gasAnim1;
    private GameObject _gasAnim2;
    
    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        
        _gasAnim1 = transform.Find("Gas1").gameObject;
        _gasAnim2 = transform.Find("Gas2").gameObject;

        _gasAnim1.SetActive(true);
        _gasAnim2.SetActive(true);
    }

    protected override void Explode()
    {
        _gasAnim1.SetActive(false);
        _gasAnim2.SetActive(false);
        
        base.Explode();
    }
}
