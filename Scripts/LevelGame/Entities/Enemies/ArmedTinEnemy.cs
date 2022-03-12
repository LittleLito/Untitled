using UnityEngine;

public class ArmedTinEnemy : TinEnemy
{
    public override EnemyType Type => EnemyType.ArmedTinEnemy;
    private GameObject _sGatling1;
    private GameObject _sGatling2;


    public override void Init(Vector3 pos)
    {
        base.Init(pos);

        _sGatling1 = transform.Find("SGatling1").gameObject;
        _sGatling1.SetActive(true);
        _sGatling1.GetComponent<EnemySGatling>().Init();
        _sGatling2 = transform.Find("SGatling2").gameObject;
        _sGatling2.SetActive(true);
        _sGatling2.GetComponent<EnemySGatling>().Init();

    }

    protected override void Explode()
    {
        _sGatling1.SetActive(false);
        _sGatling2.SetActive(false);
        base.Explode();
    }

}