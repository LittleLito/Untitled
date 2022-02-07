using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject LocationManager;
    
    // 速度
    public float Speed;
    // 能量值
    public int InitEnergyPoints;
    private int _energyPoints;
    public int EnergyPoints
    {
        get => _energyPoints;
        set
        {
            _energyPoints = value;
            UIManager.Instance.UpdateEnergyPoints(value);

            _energyUpdateAction?.Invoke();
        }
    }
    // 能量数值更新时的事件
    private UnityAction _energyUpdateAction;
    // 生命值
    [FormerlySerializedAs("InitHealth")] public float InitMaxHealth;
    private float _health;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            UIManager.Instance.UpdatePlayerHealth(value);

            if (_health <= 0)
            {
                Explode(3f);
            }
        }
    }
    
    // Game Over线
    public float DeadlineY => -7f;
    // 动画器
    private Animator _animator;
    // 渲染器
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Init()
    {
        // 装备位置管家对象
        LocationManager = Instantiate(GameManager.Instance.GameConfig.LocationManager, transform, false);
        LocationManager.transform.position = LocationManager.transform.parent.position;
        LocationManager.GetComponent<LocationManager>().Init("Player");

        // 能量
        EnergyPoints = InitEnergyPoints;
        // 生命
        Health = InitMaxHealth;
        // 查找组件
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Player") && EnergyPoints >= 1 && LevelManager.Instance.LevelState == LevelState.InGame)
        {
            Move();
        }
    }

    /// <summary>
    /// 平滑移动玩家飞机（关卡开始和结束时）
    /// </summary>
    /// <param name="destY"></param>
    public void MovePlayerAuto(float destY)
    {
        StartCoroutine(DoMove(destY));
    }
    private IEnumerator DoMove(float destY)
    {
        // 目标位置和方向
        Vector3 dest = new Vector3(transform.position.x, destY, 0);
        Vector3 direction = (dest - transform.position).normalized;
        
        // 移动
        while (Vector3.Distance(dest, transform.position) > 0.3f)
        {
            yield return new WaitForSeconds(0.02f);
            transform.Translate(direction * 0.2f); // 飞
        }
        
        Init();
    }
    
    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        var position = transform.position;
        
        if (Time.frameCount % 8 == 0) {  // 减少能量
            ReduceEnergyByMove();
        }
        
        position =
            new Vector3(
                Mathf.Clamp(
                    position.x + Input.GetAxis("Horizontal") * Speed * Time.deltaTime, 
                    -7.72f, 9
                ),  // 水平移动
                Mathf.Clamp(
                    position.y + Input.GetAxis("Vertical") * Speed * Time.deltaTime,
                    -6.58f, 5.34967f
                ),  // 垂直移动
                position.z
            );
        transform.position = position;
    }

    /// <summary>
    /// 因移动而耗费能量
    /// </summary>
    private void ReduceEnergyByMove()
    {
        var w = ConvertBool(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        var a = ConvertBool(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
        var d = ConvertBool(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));
        var s = ConvertBool(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));

        EnergyPoints -= Mathf.Min(w + a + d + s, EnergyPoints);
    }

    private static int ConvertBool(bool b)
    {
        return b ? 1 : 0;
    }

    /// <summary>
    /// 碰撞检测 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Energy"))  // 能量
        {
            other.gameObject.GetComponent<Energy>().Collect();
        }
        else if (other.gameObject.CompareTag("Enemy"))  // 撞到敌机
        {
            //  比谁硬♂ 
            var damage = Mathf.Min(Health, other.gameObject.GetComponent<EnemyBase>().Health);
            other.gameObject.GetComponent<EnemyBase>().Hit(damage, true);
            Health -= damage;
            
            LevelManager.Instance.Stats.IncreaseStat(StatType.Absorbed, damage);
        }
    }

    /// <summary>
    /// 添加能量数值更新事件的监听
    /// </summary>
    public void AddEnergyUpdateActionListener(UnityAction action)
    {
        _energyUpdateAction += action;
    }
    
    /// <summary>
    /// 生命值小于等于0后执行的操作
    /// </summary>
    protected virtual void Explode(float scale)
    {
        Destroy(LocationManager);
        Destroy(gameObject, 0.88f);
        _animator.runtimeAnimatorController = GameManager.Instance.GameConfig.Explosion;
        transform.localScale = new Vector3(scale, scale, 0);
        gameObject.tag = "Nothing";
    }
    
}