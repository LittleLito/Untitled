using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;
// ReSharper disable IdentifierTypo

public class IKunEnemy : EnemyBase
{
    public override float MaxHealth => 200;

    public override int WEIGHT => 20;

    public override int LEVEL => 5;

    protected override float _speedRange
    {
        get
        {
            return State switch
            {
                3 => Random.Range(6.0f, 6.5f),
                _ => Random.Range(2.0f, 2.4f)
            };
        }
    }    
    public override EnemyType Type => EnemyType.IKunEnemy;
    public Sprite ikunEnemy2;
    public Sprite ikunEnemy3;
    protected override Sprite DamagedImgNo2 => ikunEnemy2;
    protected override Sprite DamagedImgNo3 => ikunEnemy3;
    protected override float _explosionScale => 1.5f;

    private Basketball basketball;
    public TrailRenderer trail1, trail2;
    public Light2D light1, light2;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    [NonSerialized]
    public int State;
    private double _changeTime;
    private bool _b;

    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        
        _changeTime = 0;
        _b = false;
        trail1.enabled = false;
        trail2.enabled = false;
        light1.intensity = 0;
        light2.intensity = 0;

        basketball = Instantiate(GameManager.Instance.GameConfig.Basketball, transform).GetComponent<Basketball>();
        basketball.Init(new Vector3(0, -0.472f, 0), this);
        
        if (LevelManager.Instance.LevelState != LevelState.InGame) return;
        State = 1;
        // 开始打篮球
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    protected override void Update()
    {
        if (Health <= 0) return;

        // 在到家前移动
        if (transform.position.y > GameConfig.DeadlineY)
        {
            Move();
        }
        // 到家了
        else
        {
            // 改变关卡状态
            if (LevelManager.Instance.LevelState == LevelState.InGame)
            {
                // 厉不厉害你坤哥
                audioSource.Stop();
                audioSource.clip = audioClips[3];
                audioSource.Play();

                LevelManager.Instance.LevelState = LevelState.Over;
                
                // 移动摄像机
                Camera.main.transform.DOMoveY(-4.88f, 3.5f);
            }
            
            // 继续飞行
            if (transform.position.y > -11.5f)
            {
                transform.Translate(0, -Time.deltaTime, 0);
            }
            // 进家，飞行结束
            else
            {
                LevelManager.Instance.LevelState = LevelState.Conclusion;
            }
        }
    }

    protected override void Move()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame) return;
        
        switch (State)
        {
            case 1:
                transform.Translate(0, - FixedSpeed * Time.deltaTime * 0.1f, 0);
                break;
            case 2:
                if (!_b)
                {
                    tag = "Nothing";
                    
                    // 你干嘛
                    audioSource.Stop();
                    audioSource.clip = audioClips[1];
                    audioSource.Play();
                    Invoke(nameof(ChangeStateTo3), 4.1f);
                    _b = true;
                }
                break;
            case 3:
                // 匀加速
                Speed = (float)(_speedRange * (Time.timeAsDouble - _changeTime));
                transform.Translate(0, - FixedSpeed * Time.deltaTime * 0.1f, 0);
                break;
        }
    }

    private void ChangeStateTo3()
    {
        State = 3;
        _changeTime = Time.timeAsDouble;
        // 鸡你太美~
        audioSource.Stop();
        audioSource.clip = audioClips[2];
        audioSource.Play();
        tag = "Enemy";
        
        trail1.enabled = true;
        trail2.enabled = true;
        light1.intensity = 2;
        light2.intensity = 2;
    }

    public override void Hit(float damage, MonoBehaviour source, bool fromPlayer)
    {
        if ((source is MissileBase && State == 1) || State == 2) return;
        base.Hit(damage, source, fromPlayer);
    }

    protected override void Explode()
    {
        State = 1;
        trail1.enabled = false;
        trail2.enabled = false;
        light1.intensity = 0;
        light2.intensity = 0;

        try
        {
            basketball.Explode();
        }
        catch (MissingReferenceException)
        {
        }
        
        base.Explode();
    }}
