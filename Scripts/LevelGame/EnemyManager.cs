using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    // 所有敌机种类 
    public List<EnemyType> LevelEnemyTypes;
    // 所有可用敌机（根据所有敌机种类）
    private readonly List<EnemyBase> _levelEnemiesAvailable = new List<EnemyBase>();

    // 所有敌机
    public List<EnemyBase> Enemies { get; } = new List<EnemyBase>();

    // 总生命值
    private float _maxHealthSum;
    // 当前生命值
    private float _currentHealthSum;

    // 上次刷新时间
    private double _lastSpawnTime;

    // 出发点坐标
    private float _setUpX;
    private float _setUpY;

    private void Awake()
    {
        Instance = this;
    }

    public void InitLevelInfo()
    {
        // 通过关卡配置信息决定到访敌机
        LevelEnemyTypes = LevelManager.Instance.LevelInfo.Enemies.AsQueryable().Cast<EnemyType>().ToList();
    }

    public void Init()
    {
        foreach (var type in LevelEnemyTypes)
        {
            _levelEnemiesAvailable.Add(GetEnemyByType(type).GetComponent<EnemyBase>());
        }

        _maxHealthSum = 0.01f;
        _lastSpawnTime = Time.timeAsDouble + 14;
    }

    private void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame) return;

        // 没到达最大波数时
        if (LevelManager.Instance.WaveNum < LevelManager.Instance.LevelInfo.MaxWaveNum - 1)
        {
            AutoSpawnWave(1);
        }
        // 即将最大一波
        else if (LevelManager.Instance.WaveNum == LevelManager.Instance.LevelInfo.MaxWaveNum - 1)
        {
            AutoSpawnWave(10);
        }
        // 最大一波已过，敌机被全部击杀
        else if (Enemies.Count == 0)
        {
            if (LevelManager.Instance.LevelInfo.IsBoss)
            {
                LevelManager.Instance.LevelState = LevelState.Boss;
            }
            else
            {
                LevelManager.Instance.LevelState = LevelState.Over;
            
                Invoke(nameof(LevelPassMovePlayer), 3);
                Invoke(nameof(LevelPassMoveCamera), 3.5f);
            }
        }
    }

    /// <summary>
    /// 更新实时生命值
    /// </summary>
    public void UpdateCurrentHealthSum()
    {
        _currentHealthSum = 0;

        foreach (var enemy in Enemies)
        {
            _currentHealthSum += enemy.Health;
        }
    }

    /// <summary>
    /// 刷新新的一波
    /// </summary>
    private void AutoSpawnWave(float difficulty)
    {
        if ((_currentHealthSum / _maxHealthSum >= Random.Range(0.4f, 0.55f) ||
             Time.timeAsDouble - _lastSpawnTime <= 6) &&
            Time.timeAsDouble - _lastSpawnTime <= Random.Range(24f, 26f)) return;

        // 场上敌机生命小于总生命值0.4~0.55且距离上次刷新大于6s  或  距离上次刷新超过24s~26s
        // 刷新
        var sumLv = 0; // 实时等级总和
        var maxLv = ((LevelManager.Instance.WaveNum + 1) * 0.8 / 2 + 1) * difficulty; // 最高等级和
        // 符合等级要求（能出的怪）
        var enemiesAvailableNow = _levelEnemiesAvailable.Where(enemy => enemy.LEVEL <= maxLv).ToList();

        // 每波上限50只
        for (var i = 0; i < 50; i++)
        {
            if (sumLv <= maxLv)
            {
                var enemyBase = ToolFuncs.RandomEnemyWithWeight(enemiesAvailableNow);
                if (enemyBase is null)
                {
                    print("Null Enemy!");
                    break;
                }
                CreateEnemy(enemyBase.Type);

                sumLv += enemyBase.LEVEL;
            }
            else break;

        }

        // 更新刷新时间
        _lastSpawnTime = Time.timeAsDouble;

        // 更新波数 
        LevelManager.Instance.WaveNum += 1;

        // 更新最大生命值
        _maxHealthSum = 0;
        foreach (var enemy in Enemies)
        {
            _maxHealthSum += enemy.MaxHealth;
        }

        // 更新实时生命值
        UpdateCurrentHealthSum();
    }

    /// <summary>
    /// 生成敌机
    /// </summary>
    /// <returns></returns>
    private void CreateEnemy(EnemyType type)
    {
        // 确定初始位置
        _setUpX = Random.Range(-7.5f, 8.8f);
        _setUpY = Random.Range(6.8f, 8.0f);
        var enemy = PoolManager.Instance.GetGameObj(GetEnemyByType(type), transform)
            .GetComponent<EnemyBase>();
        enemy.Init(new Vector3(_setUpX, _setUpY, 0));
    }

    /// <summary>
    /// 展示来犯敌机
    /// </summary>
    public void UpdateEnemyShow()
    {
        foreach (var type in LevelEnemyTypes)
        {
            CreateEnemy(type);
        }
    }

    /// <summary>
    /// 添加敌机
    /// </summary>
    /// <param name="enemyBase"></param>
    public void AddEnemy(EnemyBase enemyBase)
    {
        Enemies.Add(enemyBase);
    }

    /// <summary>
    /// 移除敌机
    /// </summary>
    /// <param name="enemyBase"></param>
    public void RemoveEnemy(EnemyBase enemyBase)
    {
        Enemies.Remove(enemyBase);
    }

    /// <summary>
    /// 清除所有敌机
    /// </summary>
    public void ClearEnemy()
    {
        while (Enemies.Count > 0)
        {
            Enemies[0].Recycle();
        }
    }
    
    // 以下两个方法用于通关后的效果
    private void LevelPassMovePlayer()
    {
        PlayerManager.Instance.transform.DOMoveY(15, 6)
            .OnComplete(() => LevelManager.Instance.LevelState = LevelState.Conclusion);
    }
    private void LevelPassMoveCamera()
    {
        Camera.main.transform.DOMoveY(3.1f, 2);
    }

    /// <summary>
    /// 通过类型获取装备的预制体
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetEnemyByType(EnemyType type)
    {
        return type switch
        {
            EnemyType.NormalEnemy => GameManager.Instance.GameConfig.NormalEnemy,
            EnemyType.ConeEnemy => GameManager.Instance.GameConfig.ConeEnemy,
            EnemyType.TinEnemy => GameManager.Instance.GameConfig.TinEnemy,
            EnemyType.FlashEnemy => GameManager.Instance.GameConfig.FlashEnemy,
            EnemyType.ArmedConeEnemy => GameManager.Instance.GameConfig.ArmedConeEnemy,
            EnemyType.ArmedTinEnemy => GameManager.Instance.GameConfig.ArmedTinEnemy,
            _ => null
        };
    }
}

public enum EnemyType
{
    NormalEnemy = 1,
    ConeEnemy = 2,
    TinEnemy = 3,
    FlashEnemy = 4,
    ArmedConeEnemy = 5,
    ArmedTinEnemy = 6
}