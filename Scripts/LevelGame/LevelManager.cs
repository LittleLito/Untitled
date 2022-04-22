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
    MoveForward = 1,
    // 选卡
    Selecting = 2,
    // 摄像机回归
    MoveBack = 3,
    // 开始
    Start = 4,
    // 游戏中
    InGame = 5,
    // 游戏到boss场景转换
    GameToBoss = 6,
    // Boss
    Boss = 7,
    // 结束
    Over = 8,
    // 总结
    Conclusion = 9
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
    private int[] _levelNum
    {
        set => UIManager.Instance.UpdateLevelNum(value[0], value[1]);
    }
    // 波数
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
                    Camera.main.transform.DOMoveY (3.1f, 3.5f).OnComplete(() => LevelState = LevelState.Selecting);
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
                case LevelState.GameToBoss:
                    Stats.PauseTiming();
                    
                    // 生成boss
                    var boss = Instantiate(BossManager.Instance.Boss.gameObject, new Vector3(0, 9, 0),
                            Quaternion.identity).GetComponent<BossBase>();
                    // boss状态栏
                    UIManager.Instance.BossBarPanel.SetVisible(true);
                    // 移动入场
                    boss.transform.DOMoveY(3.14f, 8).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        // 入场结束后出示warning
                        UIManager.Instance.BossBarPanel.ShowWarning(() =>
                        {
                            // 初始化
                            boss.Init(Vector3.zero);
                            GetComponent<AudioSource>().volume = 0.1f;
                            GetComponent<AudioSource>().Play();
                            LevelState = LevelState.Boss;
                            Stats.RestartTiming();

                        });
                    });
                    break;
                case LevelState.Boss:
                    break;
                case LevelState.Over:
                    // 掐表计算游戏时间
                    Time = Stats.GetTime();
                    // 停止生产能量
                    FallingEnergyManager.Instance.StopCreate();
                    // 停止生产回复果
                    RecoverFruitManager.Instance.StopCreate();
                    // 计算数据
                    UIManager.Instance.LoadGameOverPanel();
                    // boss状态栏（如果有）
                    UIManager.Instance.BossBarPanel.SetVisible(true);
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
        EnemyManager.Instance.InitLevelInfo();
        BossManager.Instance.InitLevelInfo();
        UIManager.Instance.Init();
        _levelNum = new[] { GameData.TargetChapterNum, GameData.TargetLevelNum };
        WaveNum = 0;
        Stats.InitStats();
        LevelState = LevelState.MoveForward;
    }

    // Update is called once per frame
    private void Update()
    {
        if ((LevelState == LevelState.InGame || LevelState == LevelState.Boss) && UnityEngine.Time.frameCount % 100 == 0)
        {
            Camera.main.transform.position = new Vector3(0, -0.9f, -10);
        }
    }
}
