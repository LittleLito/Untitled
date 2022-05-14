using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    // 最大选卡数
    public int maxChosenNum;

    // 天数
    public TMP_Text levelNumText;
    // 波数
    public TMP_Text waveNumText;
    // 设置面板
    public SettingsPanel settingsPanel;
    // 结束面板
    public GameOverPanel gameOverPanel;
    // Boss面板
    public BossBarPanel bossBarPanel;
    // 卡槽
    public RectTransform seedBank;
    public Transform group;
    public TMP_Text energyPoints;
    public TMP_Text playerHealth;
    public Wrench wrench;
    // 卡片仓库
    public Transform seedStorage;
    public Transform canvas;
    // 开始倒计时文本
    public TMP_Text levelStartText;
    public TMP_FontAsset agencyB;
    public TMP_FontAsset lxgwB;
    // 已选卡片
    private readonly List<GameObject> _chosenCard = new List<GameObject>();
    // 游戏时已选卡片
    private UIGameCard _currentGameCard;
    public UIGameCard CurrentGameCard
    {
        set
        {
            if (_currentGameCard != null && _currentGameCard != value)
            {
                _currentGameCard.HasEquip = false;
            }

            _currentGameCard = value;
        }
    }


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    public void Init()
    {
        gameOverPanel.Init();
        if (LevelManager.Instance.LevelInfo.IsBoss)
        {
            bossBarPanel.Init();
        }
        
        // 更新仓库卡片
        foreach (EquipType type in Enum.GetValues(typeof(EquipType)))
        {
            // 初始化每张卡片
            var card = Instantiate(GameManager.Instance.GameConfig.Card, canvas);
            var script = card.AddComponent<UIShowCard>();
            script.Type =  type;
            script.IsChosen = false;
            script.Init();
            script.UpdatePositionInStorage();
        }
        
        // 设置开始倒计时文本
        levelStartText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingsPanel.SetVisible(Time.timeScale.Equals(1));
        }
    }

    /// <summary>
    /// 移动卡片仓库
    /// </summary>
    public void MoveSeedStorage(float destY, float duration)
    {
        seedStorage.DOMoveY(destY, duration);
    }
    
    /// <summary>
    /// 添加选择卡片
    /// </summary>
    /// <param name="card"></param>
    public void AddChosenCard(GameObject card)
    {
        if (_chosenCard.Count >= maxChosenNum) return;
        _chosenCard.Add(card);
        card.transform.SetParent(group);
        
        UpdateSeedBankHeight();
    }

    /// <summary>
    /// 移除所选卡片
    /// </summary>
    /// <param name="card"></param>
    public void RemoveChosenCard(GameObject card)
    {
        _chosenCard.Remove(card);
        card.transform.SetParent(canvas);
        card.GetComponent<UIShowCard>().UpdatePositionInStorage();
        
        UpdateSeedBankHeight();
    }

    /// <summary>
    /// 调整卡槽的高度
    /// </summary>
    private void UpdateSeedBankHeight()
    {
        seedBank.sizeDelta = new Vector2(seedBank.sizeDelta.x, 259 + 50 * (_chosenCard.Count - 1));
        GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      }

    /// <summary>
    /// 点击开始按钮
    /// </summary>
    public void OnStartButtonClicked()
    {
        LevelManager.Instance.LevelState = LevelState.MoveBack;
    }

    /// <summary>
    /// 将选好的卡片信息传到真正游戏时的卡片上，初始化卡片和扳手
    /// </summary>
    public void OnStartCardInit()
    {
        for (var i = 0; i < group.childCount; i++)
        {
            var card = group.GetChild(i).gameObject;
            var type = card.GetComponent<UIShowCard>().EquipType;
            Destroy(card.GetComponent<UIShowCard>());
            var script = card.AddComponent<UIGameCard>();
            script.Type = type;
            script.Init();
        }
    }

    /// <summary>
    /// 显示开始倒计时
    /// </summary>
    public void ShowLevelStartText()
    {
        StartCoroutine(DoLevelStartTextShow());
    }

    private IEnumerator DoLevelStartTextShow()
    {
        var count = 3;
        levelStartText.gameObject.SetActive(true);
        levelStartText.font = agencyB;

        while (count >= 1)
        {
            levelStartText.text = count.ToString();
            yield return new WaitForSeconds(1);

            count--;
        }
        
        levelStartText.font = lxgwB;
        levelStartText.text = "起飞";
        Invoke(nameof(SetLevelStartTextInactive), 0.5f);
        
        // 关卡开始前的初始化
        PlayerManager.Instance.transform.DOMoveY(-3, 1).OnComplete(() => {
            PlayerManager.Instance.Init();
            wrench.Init();
        });
        FallingEnergyManager.Instance.StartCreate();
        RecoverFruitManager.Instance.StartCreate();
        LevelManager.Instance.LevelState = LevelState.InGame;
        EnemyManager.Instance.Init();

    }

    // 将开始倒计时文本不可见
    private void SetLevelStartTextInactive() => levelStartText.gameObject.SetActive(false);

    /// <summary>
    /// 更新关卡信息
    /// </summary>
    public void UpdateLevelNum(int chap, int lv) => levelNumText.text = "关卡" + chap + "-" + lv;

    /// <summary>
    /// 更新波数信息
    /// </summary>
    /// <param name="num"></param>
    public void UpdateWaveNum(int num) => waveNumText.text = "第" + num + "波";

    /// <summary>
    /// 更新能量点数
    /// </summary>
    /// <param name="num"></param>
    public void UpdateEnergyPoints(int num) => energyPoints.text = num.ToString();

    /// <summary>
    /// 更新玩家生命值
    /// </summary>
    /// <param name="num"></param>
    public void UpdatePlayerHealth(float num) => playerHealth.text = num.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// 加载数据
    /// </summary>
    public void LoadGameOverPanel() => gameOverPanel.Load();

        /// <summary>
    /// 显示结束数据
    /// </summary>
    public void ShowGameOverPanel() => gameOverPanel.gameObject.SetActive(true);
}