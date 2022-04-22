using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wrench : MonoBehaviour
{
    private Button _button;
    private Transform _wrenchImg;
    private GameObject _indicatorImg;
    
    // 是否持有扳手
    private bool _hasWrench;
    public bool HasWrench
    {
        get => _hasWrench;
        set
        {
            _hasWrench = value;
            // 需要卸装备
            if (_hasWrench)
            {
                _wrenchImg.localRotation = Quaternion.Euler(0, 0, -30);

                _button.interactable = false;
            }
            // 放回扳手
            else
            {
                _wrenchImg.localRotation = quaternion.Euler(0, 0, 0);
                _wrenchImg.position = _wrenchImg.parent.position;

                _button.interactable = true;
            }
        }
    }

    public void Init()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickWrenchButton);
        _button.interactable = true;
        _wrenchImg = transform.Find("WrenchImage");
        _indicatorImg = transform.Find("WrenchImage/IndicatorImage").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;
        if (PlayerManager.Instance == null) return;
        if (!HasWrench) return;
        
        _wrenchImg.position = Input.mousePosition - new Vector3(-10, 45, 0);

        // 获取位置
        var location = PlayerManager.Instance.LocationManager.GetComponent<LocationManager>()
            .GetLocationByMouse();

        // 靠近网格，位置不为空且有装备，需显示指示器
        if (location != null && location.EquipBase != null)
        {
            _indicatorImg.SetActive(true);
            _indicatorImg.transform.position = Camera.main.WorldToScreenPoint(location.GetWorldPosition());
                
            // 点击左键，拆卸装备
            if (Input.GetMouseButtonDown(0))
            {
                // 卸掉
                location.EquipBase.Recycle();
                location.EquipBase = null;

                // 状态还原
                HasWrench = false;
                _indicatorImg.SetActive(false);
            }
        }
        // 离开网格，指示器取消
        else
        {
            _indicatorImg.SetActive(false);

        }

        // 点击右键收回扳手
        if (Input.GetMouseButtonDown(1))
        {
            HasWrench = false;
            _indicatorImg.SetActive(false);
        }
    }

    private void OnClickWrenchButton()
    {
        HasWrench = true;
    }
}