using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 卡片四种状态
/// </summary>
public enum GameCardState
{
    // 有能量，不在冷却
    CanPlace,

    // 有能量，在冷却
    NoCD,

    // 没有能量，不在冷却
    NoEnergy,

    // 都没有
    Neither
}

public class UIGameCard : UICard, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{   
    public EquipType Type;
    public override EquipType EquipType { get => Type; }

    // 现在的CD值，用于计算
    private float _currentCd;

    // 状态
    private GameCardState _gameCardState = GameCardState.Neither;

    public GameCardState GameCardState
    {
        get => _gameCardState;
        set
        {
            // 如果状态不变或不需更新，则不更新
            if (_gameCardState == value || (_gameCardState == GameCardState.Neither && value == GameCardState.NoCD))
            {
                return;
            }

            switch (value)
            {
                case GameCardState.CanPlace: // 不在冷却
                    _maskImg.fillAmount = 1;
                    _cardImg.color = Color.white;
                    break;
                case GameCardState.NoCD: // 在冷却，有遮罩
                    if (_gameCardState == GameCardState.Neither) return;

                    // CD开始，阴影完全遮罩
                    _maskImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                    _maskImg.fillAmount = 1;
                    StartCD(_equipScript.CD);

                    _cardImg.color = new Color(0.75f, 0.75f, 0.75f);
                    break;
                case GameCardState.NoEnergy: // 没有能量
                    _maskImg.fillAmount = 1;
                    _cardImg.color = new Color(0.75f, 0.75f, 0.75f);
                    break;
                case GameCardState.Neither: // 啥都没有
                    // CD开始，阴影完全遮罩，
                    _maskImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                    _maskImg.fillAmount = 1;
                    StartCD(_equipScript.CD);
                    _cardImg.color = new Color(0.75f, 0.75f, 0.75f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _gameCardState = value;
        }
    }


    // 是否在冷却中
    private bool _inCD;

    // 是否正在持有装备
    private bool _hasEquip;

    public bool HasEquip
    {
        get => _hasEquip;
        set
        {
            _hasEquip = value;
            if (_hasEquip) // 持有装备
            {
                // 跟随装备
                _equip = PoolManager.Instance.GetGameObj(_prefab, EquipManager.Instance.transform);
                _equip.GetComponent<EquipBase>().Create(false, Vector3.one);

                // 点击效果
                _maskImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

                //  置空上一个卡片状态
                UIManager.Instance.CurrentGameCard = this;
            }
            else // 不持有
            {
                _maskImg.color = new Color(1, 1, 1, 0);
                _equip = null;
            }
        }
    }

    // 用来创建的装备
    private GameObject _equip;

    // 用来显示的装备
    private GameObject _equipIndicator;


    // Start is called before the first frame update
    public override void Init()
    {
        base.Init();
        
        PlayerManager.Instance.AddEnergyUpdateActionListener(CheckState);
        CancelPlace();
        GameCardState = GameCardState.CanPlace;
    }

    // Update is called once per frame
    private void Update()
    {
        // 如果正在持有装备准备放置
        if (HasEquip && _equip != null)
        {
            // 鼠标坐标
            var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            // 玩家网格
            var locationManager = PlayerManager.Instance.LocationManager.GetComponent<LocationManager>();
            // 鼠标所在目标位置
            var targetLocation = locationManager.GetLocationByWorldPos(mousePos);

            // 跟随鼠标
            _equip.transform.position = mousePos;

            // 如果距离网格比较近，需要出现透明指示器
            if (targetLocation.EquipBase == null && Vector2.Distance(mousePos, targetLocation.GetWorldPosition()) < 0.3f)
            {
                if (_equipIndicator == null)
                {
                    // 没有实例化，进行实例化
                    _equipIndicator = PoolManager.Instance.GetGameObj(_prefab, locationManager.transform);
                    _equipIndicator.GetComponent<EquipBase>().Create(true, targetLocation.GetWorldPosition());
                }
                else
                {
                    // 已经实例化，调整位置
                    _equipIndicator.transform.position = targetLocation.GetWorldPosition();
                }

                // 点击鼠标放置
                if (Input.GetMouseButtonDown(0))
                {
                    _equipIndicator.GetComponent<EquipBase>().Recycle();
                    _equipIndicator = null;
                    
                    // 放置装备，绑定位置
                    _equip.transform.parent = locationManager.transform;
                    _equip.transform.position = targetLocation.GetWorldPosition();
                    targetLocation.EquipBase = _equip.GetComponent<EquipBase>();

                    _inCD = true;
                    _equip.GetComponent<EquipBase>().Place();
                    _hasEquip = false;
                    _equip = null; // 不持有了
                }
            }
            // 离开网格，指示器取消
            else
            {
                if (_equipIndicator != null)
                {
                    _equipIndicator.GetComponent<EquipBase>().Recycle();
                    _equipIndicator = null;
                }
            }

            // 如果点击右键，取消持有状态
            if (Input.GetMouseButtonDown(1))
            {
                CancelPlace();
            }
        }
    }

    /// <summary>
    /// 取消放置
    /// </summary>
    private void CancelPlace()
    {
        _equip?.GetComponent<EquipBase>().Recycle();
        HasEquip = false;
        if (_equipIndicator == null) return;
        _equipIndicator.GetComponent<EquipBase>().Recycle();
        _equipIndicator = null;
    }

    /// <summary>
    /// 鼠标移入
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameCardState != GameCardState.CanPlace) return;
        if (HasEquip) return;
        _maskImg.color = new Color(1f, 1f, 1f, 0.3f);
    }

    /// <summary>
    /// 鼠标移出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameCardState != GameCardState.CanPlace) return;
        if (HasEquip) return;
        _maskImg.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// 鼠标点击效果
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameCardState != GameCardState.CanPlace) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!HasEquip)
        {
            HasEquip = true; //  持有装备
        }
    }

    /// <summary>
    /// 开始CD
    /// </summary>
    private void StartCD(float startCD)
    {
        // CD开始，阴影完全遮罩，
        _maskImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        _maskImg.fillAmount = 1;

        // 开始计时
        _currentCd = startCD;
        StartCoroutine(CalculateCD(startCD));
    }

    /// <summary>
    /// CD效果及计算协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator CalculateCD(float startCD)
    {
        // 当前冷却值大于0时
        while (_currentCd >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            _maskImg.fillAmount -= (1 / startCD) * 0.1f; // 掀开一点阴影
            _currentCd -= 0.1f; // 继续冷却
        }

        // 冷却结束，可以装备
        _inCD = false;
        _maskImg.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// 检测状态
    /// </summary>
    private void CheckState()
    {
        GameCardState = _inCD switch
        {
            // 有能量，不在冷却
            false when PlayerManager.Instance.EnergyPoints >= _equipScript.Cost => GameCardState.CanPlace,
            // 有能量，在冷却
            true when PlayerManager.Instance.EnergyPoints >= _equipScript.Cost => GameCardState.NoCD,
            // 没有能量，不在冷却
            false when PlayerManager.Instance.EnergyPoints < _equipScript.Cost => GameCardState.NoEnergy,
            // 都没有
            _ => GameCardState.Neither
        };
    }
}