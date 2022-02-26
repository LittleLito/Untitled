using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedConeEnemy : ConeEnemy
{
    public override EnemyType Type => EnemyType.ArmedConeEnemy;
        
    private GameObject _sGatling;

    public override void Init(Vector3 pos)
    {
        base.Init(pos);

        _sGatling = transform.Find("SGatling").gameObject;
        _sGatling.SetActive(true);
    }

    protected override void Explode()
    {
        _sGatling.SetActive(false);
        base.Explode();
    }
}
