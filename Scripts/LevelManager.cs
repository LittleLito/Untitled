using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 关卡状态
/// </summary>
public enum LevelState
{
    // 摄像机前移
    MoveForward,
    // 选卡
    Selecting,
    // 摄像机回归
    MoveBack,
    // 开始
    Start,
    // 游戏中
    InGame,
    // 结束
    Over,
    // 总结
    Conclusion
}
public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    // 关卡配置
    public LevelInfo LevelInfo;
    
    // 统计数据
    public readonly Stats Stats = new Stats();
    public double Time;
    // 关卡编号
    private int[] _levelNum;
    public int[] LevelNum
    {
        get => _levelNum;
        set
        {
            _levelNum = value;
            UIManager.Instance.UpdateLevelNum(value[0], value[1]);
        }
    }
    // 波数
    //public int MaxWaveNum { get; private set; }
    private int _waveNum;
    public int WaveNum
    {
        get => _waveNum;
        set
        {
            _waveNum = value;
            UIManager.Instance.UpdateWaveNum(_waveNum);
        }
    }
    // 关卡状态
    private LevelState _levelState;
    public LevelState LevelState
    {
        get => _levelState;
        set
        {
            _levelState = value;
            switch (_levelState)
            {
                case LevelState.MoveForward:
                    // 显示来犯敌机
                    EnemyManager.Instance.UpdateEnemyShow();
                    // 移动摄像机
                    Camera.main.transform.position = new Vector3(0, -0.9f, -10);
                    Camera.main.transform.DOMoveY(3.1f, 3.5f).OnComplete(() => LevelState = LevelState.Selecting);
                    break;
                case LevelState.Selecting:
                    // 摄像机不动
                    CameraController.Instance.transform.position = new Vector3(0, 3.1f, -10);
                    // 飞出卡片仓库
                    UIManager.Instance.MoveSeedStorage(15f, 1f);
                    break;
                case LevelState.MoveBack:
                    // 飞走卡片仓库
                    UIManager.Instance.MoveSeedStorage(-787, 1f);
                    // 摄像机回归
                    Camera.main.transform.DOMoveY(-0.9f, 3.5f).OnComplete(() => LevelState = LevelState.Start);
                    break;
                case LevelState.Start:
                    // 清除展示敌机
                    EnemyManager.Instance.ClearEnemy();
                    // 开始字幕
                    UIManager.Instance.ShowLevelStartText();
                    // 初始化卡片
                    UIManager.Instance.OnStartCardInit();
                    break;
                case LevelState.InGame:
                    Stats.StartTiming();
                    break;
                case LevelState.Over:
                    // 掐表计算游戏时间
                    Time = Stats.GetTime();
                    // 停止生产能量
                    FallingEnergyManager.Instance.StopCreate();
                    // 计算数据
                    UIManager.Instance.LoadGameOverPanel();
                    break;
                case LevelState.Conclusion:
                    PoolManager.Instance.ClearGameObj();
                    UIManager.Instance.ShowGameOverPanel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void StartLevel(LevelInfo levelInfo)
    {
        LevelInfo = levelInfo;
        UIManager.Instance.Init();
        EnemyManager.Instance.InitLevelInfo();
        LevelNum = new[] { GameData.TargetChapterNum, GameData.TargetLevelNum };
        WaveNum = 0;
        Stats.InitStats();
        LevelState = LevelState.MoveForward;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
