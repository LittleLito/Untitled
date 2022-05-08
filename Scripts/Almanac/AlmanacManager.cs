using System;
using UnityEngine;
using UnityEngine.UI;

public enum AlmanacMode
{
    Null,
    Equipment,
    Projectile,
    Enemy,
    EnemyEquipment
}

public class AlmanacManager : MonoBehaviour
{
    public static AlmanacManager Instance;

    private AlmanacMode _almanacMode;
    public AlmanacMode AlmanacMode
    {
        set
        {
           if (_almanacMode == value) return;

           _almanacMode = value;
           
           switch (value)
           {
                case AlmanacMode.Equipment:
                    UpdateEquipCardStorage();
                    UpdateInfoAction = UpdateEquipCardInfo;
                    break;
                case AlmanacMode.Projectile:
                    UpdateProjectileCardStorage();
                    UpdateInfoAction = UpdateProjectileCardInfo;
                    break;
                case AlmanacMode.Enemy:
                    UpdateEnemyCardStorage();
                    UpdateInfoAction = UpdateEnemyCardInfo;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
           }
        }
    }
    // 动态信息更新方法
    private Action UpdateInfoAction;

    // 现在展示的卡片
    private UIAlmanacCard _currentCard;
    public UIAlmanacCard CurrentCard
    {
        set
        {
            if (value.Equals(_currentCard)) return;
            
            _currentCard = value;

            UpdateInfoAction();
        }
    }

    public Transform seedStorage;
    public UIAlmanacCard seedCard;
    public Text seedName;
    public Text seedConclusion;
    public Transform propertiesItems;
    public Transform propertiesValues;
    public Text seedDescription;
    public GameObject propertyInfoText;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AlmanacMode = AlmanacMode.Equipment;
        // 默认展示第一张卡片
        UpdateEquipCardInfo(1);

    }

    /// <summary>
    /// 清空卡片仓库
    /// </summary>
    private void ClearStorage()
    {
        while (seedStorage.childCount > 0)
        {
            DestroyImmediate(seedStorage.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// 更新装备卡片到仓库中
    /// </summary>
    public void UpdateEquipCardStorage()
    {
        ClearStorage();
        
        // 更新仓库卡片
        foreach (EquipType type in Enum.GetValues(typeof(EquipType)))
        {
            // 初始化每张卡片
            var card = Instantiate(GameManager.Instance.GameConfig.Card, seedStorage);
            var script = card.AddComponent<UIAlmanacCard>();
            script.Type = type;
            script.InitForEquip();
            script.UpdatePositionInStorage();
        }

    }

    /// <summary>
    /// 更新装备卡片信息
    /// </summary>
    private void UpdateEquipCardInfo()
    {
        UpdateEquipCardInfo((int) _currentCard.Type);
    }
    private void UpdateEquipCardInfo(int id)
    {
        var _currentEquipmentData = GameData.AlmanacDataOperator.EquipmentsDatas.Find(data => data.Id == id);

        // 清除所有上个卡片的属性信息
        while (propertiesItems.childCount > 0)
        {
            PoolManager.Instance.PushGameObj(propertyInfoText, propertiesItems.GetChild(0).gameObject);
            PoolManager.Instance.PushGameObj(propertyInfoText, propertiesValues.GetChild(0).gameObject);
        }
        
        // 图片
        seedCard.Type = (EquipType)_currentEquipmentData.Id;
        seedCard.InitForEquip();
        // 名称
        seedName.text = _currentEquipmentData.ChineseName;
        // 概括
        seedConclusion.text = _currentEquipmentData.Conclusion;
        
        // 属性
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesItems).GetComponent<Text>().text = "CD";
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesValues).GetComponent<Text>().text = _currentEquipmentData.CD + "s";
        foreach (var item in _currentEquipmentData.PropertyItems)
        {
            PoolManager.Instance.GetGameObj(propertyInfoText, propertiesItems).GetComponent<Text>().text = item;
        }
        foreach (var value in _currentEquipmentData.PropertyValues)
        {
            PoolManager.Instance.GetGameObj(propertyInfoText, propertiesValues).GetComponent<Text>().text = value;
        }
        
        // 描述
        seedDescription.text = _currentEquipmentData.Description;
    }
    
    /// <summary>
    /// 更新弹射物卡片到仓库中
    /// </summary>
    private void UpdateProjectileCardStorage()
    {
        ClearStorage();
        
        // 更新仓库卡片
        foreach (EquipType type in Enum.GetValues(typeof(ProjectileType)))
        {
            // 初始化每张卡片
            var card = Instantiate(GameManager.Instance.GameConfig.Card, seedStorage);
            var script = card.AddComponent<UIAlmanacCard>();
            script.Type = type;
            script.InitForProj();
            script.UpdatePositionInStorage();
        }

    }
    /// <summary>
    /// 更新弹射物卡片信息
    /// </summary>
    private void UpdateProjectileCardInfo()
    {
        var _currentEquipmentData = GameData.AlmanacDataOperator.ProjectilesDatas.Find(data => data.Id == (int) _currentCard.Type);

        // 清除所有上个卡片的属性信息
        while (propertiesItems.childCount > 0)
        {
            PoolManager.Instance.PushGameObj(propertyInfoText, propertiesItems.GetChild(0).gameObject);
            PoolManager.Instance.PushGameObj(propertyInfoText, propertiesValues.GetChild(0).gameObject);
        }
        
        // 图片
        seedCard.Type = (ProjectileType)_currentEquipmentData.Id;
        seedCard.InitForProj();
        // 名称
        seedName.text = _currentEquipmentData.ChineseName;
        // 概括
        seedConclusion.text = _currentEquipmentData.Conclusion;

        // 属性
        foreach (var item in _currentEquipmentData.PropertyItems)
        {
            PoolManager.Instance.GetGameObj(propertyInfoText, propertiesItems).GetComponent<Text>().text = item;
        }
        foreach (var value in _currentEquipmentData.PropertyValues)
        {
            PoolManager.Instance.GetGameObj(propertyInfoText, propertiesValues).GetComponent<Text>().text = value;
        }
        
        // 描述
        seedDescription.text = _currentEquipmentData.Description;

    }
    
    /// <summary>
    /// 更新敌机卡片到仓库中
    /// </summary>
    private void UpdateEnemyCardStorage()
    {
        ClearStorage();
        
        // 更新仓库卡片
        foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
        {
            // 初始化每张卡片
            var card = Instantiate(GameManager.Instance.GameConfig.Card, seedStorage);
            var script = card.AddComponent<UIAlmanacCard>();
            script.Type = type;
            script.InitForEnemy();
            script.UpdatePositionInStorage();
        }

    }
    /// <summary>
    /// 更新敌机卡片信息
    /// </summary>
    private void UpdateEnemyCardInfo()
    {
        var _currentEnemyData = GameData.AlmanacDataOperator.EnemiesDatas.Find(data => data.Id == (int)_currentCard.Type);

        // 清除所有上个卡片的属性信息
        while (propertiesItems.childCount > 0)
        {
            PoolManager.Instance.PushGameObj(propertyInfoText, propertiesItems.GetChild(0).gameObject);
            PoolManager.Instance.PushGameObj(propertyInfoText, propertiesValues.GetChild(0).gameObject);
        }
        
        // 图片
        seedCard.Type = (EnemyType)_currentEnemyData.Id;
        seedCard.InitForEnemy();
        // 名称
        seedName.text = _currentEnemyData.ChineseName;
        // 概括
        seedConclusion.text = _currentEnemyData.Conclusion;

        // 属性
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesItems).GetComponent<Text>().text = "出怪等级";
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesValues).GetComponent<Text>().text = _currentEnemyData.Level.ToString();
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesItems).GetComponent<Text>().text = "出怪权重";
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesValues).GetComponent<Text>().text = _currentEnemyData.Weight.ToString();
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesItems).GetComponent<Text>().text = "最大生命";
        PoolManager.Instance.GetGameObj(propertyInfoText, propertiesValues).GetComponent<Text>().text = _currentEnemyData.MaxHealth.ToString();

        foreach (var item in _currentEnemyData.PropertyItems)
        {
            PoolManager.Instance.GetGameObj(propertyInfoText, propertiesItems).GetComponent<Text>().text = item;
        }
        foreach (var value in _currentEnemyData.PropertyValues)
        {
            PoolManager.Instance.GetGameObj(propertyInfoText, propertiesValues).GetComponent<Text>().text = value;
        }
        
        // 描述
        seedDescription.text = _currentEnemyData.Description;
    }

}