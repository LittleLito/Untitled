using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAlmanacCard : UICard, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler 
{
    public object Type;
    public override EquipType EquipType => (EquipType)Type;
    public ProjectileType ProjectileType => (ProjectileType)Type;
    public EnemyType EnemyType => (EnemyType)Type;

    /// <summary>
    /// 给装备的初始化
    /// </summary>
    public void InitForEquip()
    {
        _prefab = EquipManager.Instance.GetEquipByType(EquipType);
        _equipScript = _prefab.GetComponent<EquipBase>();

        // 卡片图片
        _cardImg = GetComponent<Image>();
        _cardImg.sprite = EquipManager.Instance.GetCardImgByFamily(_equipScript.Family);

        // 遮罩阴影
        _maskImg = transform.Find("Mask").GetComponent<Image>();

        // 装备图片
        _equipImg = transform.Find("EquipImg").GetComponent<Image>();
        _equipImg.sprite = _equipScript.EquipImg;

        // 花费点数text
        _costText = transform.Find("Cost").GetComponent<TMP_Text>();
        _costText.text = _equipScript.Cost.ToString();

		// 能量图片
		_energySignImg = transform.Find("EnergySign").GetComponent<Image>();
        _energySignImg.sprite = _equipScript is IMoonEnergyEquip ? GameManager.Instance.GameConfig.MoonEnergySign : GameManager.Instance.GameConfig.SunEnergySign;
		
        // 运行时花费显示文本, 如果不为0，则显示
        if (_equipScript.RunCost == 0) return;
        _runCostText = transform.Find("EnergySign/RunCost").GetComponent<TMP_Text>();
        _runCostText.text = _equipScript.RunCost.ToString();
    }

    /// <summary>
    /// 给弹射物的初始化
    /// </summary>
    public void InitForProj()
    {
        _prefab = EquipManager.Instance.GetProjectileByType(ProjectileType);
        
        // 卡片图片
        _cardImg = GetComponent<Image>();
        _cardImg.sprite = GameManager.Instance.GameConfig.CardCommon;

        // 遮罩阴影
        _maskImg = transform.Find("Mask").GetComponent<Image>();

        // 装备图片
        _equipImg = transform.Find("EquipImg").GetComponent<Image>();
        _equipImg.sprite = _prefab.GetComponent<SpriteRenderer>().sprite;
        _equipImg.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        
        // 能量图片
        _energySignImg = transform.Find("EnergySign").GetComponent<Image>();
        _energySignImg.sprite = GameManager.Instance.GameConfig.SunEnergySign;
        
        // 花费点数text隐藏
        _costText = transform.Find("EnergySign/Cost").GetComponent<TMP_Text>();
        _costText.text = "";

        // 运行时花费text隐藏
        _runCostText = transform.Find("RunCost").GetComponent<TMP_Text>();
        _runCostText.text = "";

    }

    /// <summary>
    /// 给敌机的初始化
    /// </summary>
    public void InitForEnemy()
    {
        _prefab = EnemyManager.GetEnemyByType(EnemyType);
        _equipScript = _prefab.GetComponent<EquipBase>();

        // 卡片图片
        _cardImg = GetComponent<Image>();
        _cardImg.sprite = GameManager.Instance.GameConfig.CardCommon;

        // 遮罩阴影
        _maskImg = transform.Find("Mask").GetComponent<Image>();

        // 装备图片
        _equipImg = transform.Find("EquipImg").GetComponent<Image>();
        _equipImg.sprite = _prefab.GetComponent<SpriteRenderer>().sprite;

        // 花费点数text
        _costText = transform.Find("Cost").GetComponent<TMP_Text>();
        _costText.text = "";
        
        // 能量图片
        _energySignImg = transform.Find("EnergySign").GetComponent<Image>();
        _energySignImg.sprite = GameManager.Instance.GameConfig.SunEnergySign;

        // 运行时花费显示文本, 如果不为0，则显示
        _runCostText = transform.Find("EnergySign/RunCost").GetComponent<TMP_Text>();
        _runCostText.text = "";
    }
    
    /// <summary>
    /// 点击时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        AlmanacManager.Instance.CurrentCard = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _maskImg.color = new Color(1f, 1f, 1f, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _maskImg.color = new Color(1, 1, 1, 0);
    }
    
    /// <summary>
    /// 根据索引更新在仓库中的位置
    /// </summary>
    public void UpdatePositionInStorage()
    {
        GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        transform.position = new Vector3(((int)EquipType - 1) % 4 * 119 + 16, - (((int)EquipType - 1) / 4 * 53 + 16), 0) + transform.parent.position;
    }


}
