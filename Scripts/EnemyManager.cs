using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    // 所有敌机种类 
    private readonly List<EnemyType> LevelEnemyTypes = new List<EnemyType>
    {
        EnemyType.NormalEnemy,
        EnemyType.ConeEnemy,
        EnemyType.TinEnemy,
        EnemyType.FlashEnemy
    };

    // 所有可用敌机（根据所有敌机种类）
    private readonly List<EnemyBase> _levelEnemiesAvailable = new List<EnemyBase>();

    // 所有敌机
    private readonly List<EnemyBase> _enemies = new List<EnemyBase>();
    public List<EnemyBase> Enemies => _enemies;

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

    // Start is called before the first frame update
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
        if (LevelManager.Instance.WaveNum < LevelManager.Instance.MaxWaveNum)
        {
            AutoSpawnWave(1);
        }
        // 最大一波
        else if (LevelManager.Instance.WaveNum == LevelManager.Instance.MaxWaveNum)
        {
            AutoSpawnWave(10);
        }
        else
        {
            LevelManager.Instance.LevelState = LevelState.Over;
        }
    }

    /// <summary>
    /// 更新实时生命值
    /// </summary>
    public void UpdateCurrentHealthSum()
    {
        _currentHealthSum = 0;

        foreach (var enemy in _enemies)
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
        var maxLv = (LevelManager.Instance.WaveNum * 0.8 / 2 + 1) * difficulty; // 最高等级和
        // 符合等级要求（能出的怪）
        var enemiesAvailableNow = _levelEnemiesAvailable.Where(enemy => enemy.LEVEL <= maxLv).ToList();

        // 每波上限50只
        for (var i = 0; i < 50; i++)
        {
            if (sumLv <= maxLv)
            {
                var enemyBase = Tools.RandomEnemyWithWeight(enemiesAvailableNow);
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
        foreach (var enemy in _enemies)
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
        _enemies.Add(enemyBase);
    }

    /// <summary>
    /// 移除敌机
    /// </summary>
    /// <param name="enemyBase"></param>
    public void RemoveEnemy(EnemyBase enemyBase)
    {
        _enemies.Remove(enemyBase);
    }

    /// <summary>
    /// 清除所有敌机
    /// </summary>
    public void ClearEnemy()
    {
        while (_enemies.Count > 0)
        {
            _enemies[0].Recycle();
        }
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
            _ => null
        };
    }
}

public enum EnemyType
{
    NormalEnemy,
    ConeEnemy,
    TinEnemy,
    FlashEnemy
}