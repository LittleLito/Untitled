using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    // 天数
    private Text _levelNumText;
    // 波数
    private Text _waveNumText;
    // 设置面板
    private SettingsPanel _settingsPanel;
    // 结束面板
    private GameOverPanel _gameOverPanel;
    // 卡槽
    public int MaxChosenNum;
    private RectTransform _seedBank;
    private GameObject _group;
    private Text _energyPoints;
    private Text _playerHealth;
    private Wrench _wrench;
    // 卡片仓库
    private Transform _seedStorage;
    private GameObject _canvas;
    // 开始倒计时文本
    private Text _levelStartText;
    // 已选卡片
    public List<GameObject> ChosenCard = new List<GameObject>();
    // 游戏时已选卡片
    private UIGameCard _currentGameCard;
    public UIGameCard CurrentGameCard
    {
        get => _currentGameCard;
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
        // 获取组件
        _levelNumText = transform.Find("LevelInfoPanel/LevelNum").GetComponent<Text>();
        _waveNumText = transform.Find("LevelInfoPanel/WaveNum").GetComponent<Text>();
        _settingsPanel = transform.Find("SettingsPanel").GetComponent<SettingsPanel>();
        _gameOverPanel = transform.Find("GameOverPanel").GetComponent<GameOverPanel>();
        _seedBank = transform.Find("SeedBank").GetComponent<RectTransform>();
        _group = transform.Find("SeedBank/Group").gameObject;
        _energyPoints = transform.Find("SeedBank/EnergyPoints").GetComponent<Text>();
        _playerHealth = transform.Find("SeedBank/PlayerHealth").GetComponent<Text>();
        _wrench = transform.Find("SeedBank/WrenchButton").GetComponent<Wrench>();
        _seedStorage = transform.Find("SeedStorage");
        _canvas = transform.Find("SeedStorage/Canvas").gameObject;
        _levelStartText = transform.Find("LevelStartText").GetComponent<Text>();
        
        _gameOverPanel.Init();
        
        // 更新仓库卡片
        var index = 0;
        foreach (EquipType type in Enum.GetValues(typeof(EquipType)))
        {
            // 初始化每张卡片
            var card = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Card, _canvas.transform);
            var script = card.AddComponent<UIShowCard>();
            script.Type =  type;
            script.IsChosen = false;
            script.Index = index;
            script.Init();
            script.UpdatePositionInStorage();


            index++;
        }

        GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // 设置开始倒计时文本
        _levelStartText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _settingsPanel.SetVisible(Time.timeScale.Equals(1));
        }
    }

    /// <summary>
    /// 移动卡片仓库
    /// </summary>
    public void MoveSeedStorage(float destY, float duration)
    {
        _seedStorage.DOMoveY(destY, duration);
    }
    
    /// <summary>
    /// 添加选择卡片
    /// </summary>
    /// <param name="card"></param>
    public void AddChosenCard(GameObject card)
    {
        if (ChosenCard.Count >= MaxChosenNum) return;
        ChosenCard.Add(card);
        card.transform.SetParent(_group.transform);
        
        UpdateSeedBankHeight();
    }

    /// <summary>
    /// 移除所选卡片
    /// </summary>
    /// <param name="card"></param>
    public void RemoveChosenCard(GameObject card)
    {
        ChosenCard.Remove(card);
        card.transform.SetParent(_canvas.transform);
        card.GetComponent<UIShowCard>().UpdatePositionInStorage();
        
        UpdateSeedBankHeight();
    }

    /// <summary>
    /// 调整卡槽的高度
    /// </summary>
    private void UpdateSeedBankHeight()
    {
        _seedBank.sizeDelta = new Vector2(_seedBank.sizeDelta.x, 259 + 50 * (ChosenCard.Count - 1));
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
        for (int i = 0; i < _group.transform.childCount; i++)
        {
            var card = _group.transform.GetChild(i).gameObject;
            var type = card.GetComponent<UIShowCard>().EquipType;
            Destroy(card.GetComponent<UIShowCard>());
            var script = card.AddComponent<UIGameCard>();
            script.Type = type;
            script.Init();
        }
        
        _wrench.Init();
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
        _levelStartText.gameObject.SetActive(true);
        _levelStartText.font = GameManager.Instance.GameConfig.AGENCYB;

        while (count >= 1)
        {
            _levelStartText.text = count.ToString();
            yield return new WaitForSeconds(1);

            count--;
        }
        
        _levelStartText.font = GameManager.Instance.GameConfig.LXGWWENKAI_BOLD;
        _levelStartText.text = "起飞";
        Invoke(nameof(SetLevelStartTextInactive), 0.5f);
        
        // 关卡开始前的初始化
        PlayerManager.Instance.transform.DOMoveY(-3, 1).OnComplete(PlayerManager.Instance.Init);
        FallingEnergyManager.Instance.StartCreate();
        RecoverFruitManager.Instance.StartCreate();
        LevelManager.Instance.LevelState = LevelState.InGame;
        EnemyManager.Instance.Init();

    }

    // 将开始倒计时文本不可见
    private void SetLevelStartTextInactive()
    {
        _levelStartText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新关卡信息
    /// </summary>
    public void UpdateLevelNum(int chap, int lv)
    {
        _levelNumText.text = "关卡" + chap + "-" + lv;
    }

    /// <summary>
    /// 更新波数信息
    /// </summary>
    /// <param name="num"></param>
    public void UpdateWaveNum(int num)
    {
        _waveNumText.text = "第" + num + "波";
    }

    /// <summary>
    /// 更新能量点数
    /// </summary>
    /// <param name="num"></param>
    public void UpdateEnergyPoints(int num)
    {
        _energyPoints.text = num.ToString();
    }

    /// <summary>
    /// 更新玩家生命值
    /// </summary>
    /// <param name="num"></param>
    public void UpdatePlayerHealth(float num)
    {
        _playerHealth.text = num.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public void LoadGameOverPanel()
    {
        _gameOverPanel.Load();
    }

    /// <summary>
    /// 显示结束数据
    /// </summary>
    public void ShowGameOverPanel()
    {
        _gameOverPanel.gameObject.SetActive(true);
    }
}