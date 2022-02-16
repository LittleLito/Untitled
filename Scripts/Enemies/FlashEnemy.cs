using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEnemy : EnemyBase
{
    public override float MaxHealth => 100;
    public override float Speed => Random.Range(6f, 6.5f);
    public override int WEIGHT => 20;
    public override int LEVEL => 2;
    public override EnemyType Type => EnemyType.FlashEnemy;
    protected override Sprite DamagedImgNo2 => GameManager.Instance.GameConfig.FlashEnemy2;
    protected override Sprite DamagedImgNo3 => GameManager.Instance.GameConfig.FlashEnemy3;
    protected override float _explosionScale => 1.3f;
    
    private GameObject _rectMask;
    private TrailRenderer _trail;
    public float _jumpY;
    private int _state;

    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        
        _rectMask = transform.Find("Rect").gameObject;

        _trail = GetComponent<TrailRenderer>();
        _trail.enabled = true;

        _jumpY = Random.Range(4f, -2.68f);

        _state = 1;

    }

    protected override void Move()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame) return;
        
        switch (_state)
        {
            case 1:
                transform.Translate(0, -Speed * Time.deltaTime * 0.1f, 0);
                    
                if (transform.position.y < _jumpY)
                {
                    _state = 2;
                }
                break;
            case 2:
                transform.Translate(0, - Random.Range(1.5f, 2f), 0);
                _rectMask.SetActive(true);
                _trail.enabled = false;
                _state = 3;
                break;
            case 3:
                transform.Translate(0, - Random.Range(2.8f, 3.2f) * Time.deltaTime * 0.1f, 0);
                break;
        }
    }

    protected override void Explode()
    {
        _rectMask.SetActive(false);
        _trail.enabled = false;
        
        base.Explode();
    }

}
