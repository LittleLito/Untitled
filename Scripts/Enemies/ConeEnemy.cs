using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeEnemy : EnemyBase
{
    public override float MaxHealth => 80;
    public override float Speed => Random.Range(2.8f, 3.2f);
    public override int WEIGHT => 40;
    public override int LEVEL => 2;
    public override EnemyType Type => EnemyType.ConeEnemy;
    protected override Sprite DamagedImgNo1 => GameManager.Instance.GameConfig.ConeEnemy1;
    protected override Sprite DamagedImgNo2 => GameManager.Instance.GameConfig.ConeEnemy2;
    protected override Sprite DamagedImgNo3 => GameManager.Instance.GameConfig.ConeEnemy3;
    protected override float ExplodeScale => 1.4f;

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
