using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UICard : MonoBehaviour
{
    // 装备种类
    public virtual EquipType EquipType { get; }

    // 花费文本
    private Text _costText;

    // 运行时花费显示文本
    private Text _runCostText;

    // 卡片图片
    protected Image _cardImg;

    // 遮罩图片
    protected Image _maskImg;

    // 装备图片
    private Image _equipImg;
    
    // 预制体
    protected GameObject _prefab;

    // 装备脚本
    protected EquipBase _equipScript;


    public virtual void Init()
    {
        _prefab = EquipManager.Instance.GetEquipByType(EquipType);
        _equipScript = _prefab.GetComponent<EquipBase>();

        // 卡片图片
        _cardImg = GetComponent<Image>();
        _cardImg.sprite = EquipManager.Instance.GetCardImgByFamily(_equipScript.Family);

        // 遮罩阴影
        _maskImg = transform.Find("Mask").GetComponent<Image>();
        _maskImg.color = new Color(1, 1, 1, 0f);

        // 装备图片
        _equipImg = transform.Find("EquipImg").GetComponent<Image>();
        _equipImg.sprite = _equipScript.EquipImg;

        // 花费点数text
        _costText = transform.Find("Cost").GetComponent<Text>();
        _costText.text = _equipScript.Cost.ToString();

        // 运行时花费显示文本, 如果不为0，则显示
        if (_equipScript.RunCost != 0)
        {
            _runCostText = transform.Find("RunCost").GetComponent<Text>();
            _runCostText.text = _equipScript.RunCost.ToString();
        }
        


    }
}