using UnityEngine;

public class ConeEnemy : EnemyBase
{
    public override float MaxHealth => 120;
    protected override float _speedRange => Random.Range(2.4f, 2.8f);  
    public override int WEIGHT => 40;
    public override int LEVEL => 2;
    public override EnemyType Type => EnemyType.ConeEnemy;
    public Sprite coneEnemy2;
    public Sprite coneEnemy3;
    protected override Sprite DamagedImgNo2 => coneEnemy2;
    protected override Sprite DamagedImgNo3 => coneEnemy3;
    protected override float _explosionScale => 1.4f;

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
