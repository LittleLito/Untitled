using TMPro;
using UnityEngine;

// ReSharper disable InconsistentNaming

/// <summary>
/// 游戏配置
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("卡片相关")]
	[Tooltip("阳光能量")] public Sprite SunEnergySign;
	[Tooltip("月光能量")] public Sprite MoonEnergySign;
    [Tooltip("卡片")] public GameObject Card;
    [Tooltip("普通卡片")] public Sprite CardCommon;
    [Tooltip("能量生产卡片")] public Sprite CardEnlighten;
    [Tooltip("爆炸类卡片")] public Sprite CardDoom;
    [Tooltip("防御卡片")] public Sprite CardDefence;
    [Tooltip("冰冻卡片")] public Sprite CardFrozen;

    [Header("其他资源")] 
    //[Tooltip("AGENCYB")] public TMP_FontAsset AGENCYB;
    //[Tooltip("LXGWWENKAI-BOLD")] public TMP_FontAsset LXGWWENKAI_BOLD;
    [Tooltip("装备位置管理")] public GameObject LocationManager;
    [Tooltip("能量")] public GameObject Energy;
    [Tooltip("回复果")] public GameObject RecoverFruit;
    [Tooltip("回复果摇摆动画")] public RuntimeAnimatorController RecoverFruitShake;
    [Tooltip("爆炸")] public GameObject Explosion; 
    [Tooltip("爆炸效果")] public RuntimeAnimatorController ExplosionAnim;
    [Tooltip("喷射尾气")] public RuntimeAnimatorController Gas;
    [Tooltip("准星")] public GameObject Aim;
    [Tooltip("烟雾")] public GameObject Smoke;
    [Tooltip("延时炸弹动画")] public RuntimeAnimatorController DelayBombAnim;
    [Tooltip("防弹盾牌展开动画")] public RuntimeAnimatorController ShieldOpen;
    public Sprite ShieldOpenImg;
    public Sprite Shield1;
    public Sprite Shield2;
    [Tooltip("GM火箭炮枪口火焰1")] public Sprite GMRocketGunfire1;
    [Tooltip("守卫者枪口火焰")] public Sprite CattailGunfire1;

    [Header("装备")]
    [Tooltip("S型轻型机枪")] public GameObject SGatling;
    [Tooltip("能源反应堆")] public GameObject EnergyReactor;
    [Tooltip("支援弹道导弹")] public GameObject CherryMissile;
    [Tooltip("延时炸弹")] public GameObject DelayBomb;
    [Tooltip("防弹盾牌")] public GameObject Shield;
    [Tooltip("S型冰霜机枪")] public GameObject FrozenSGatling;
    [Tooltip("SP双联机枪")] public GameObject SPGatling;
    [Tooltip("守卫者")] public GameObject Cattail;
    [Tooltip("s型小型机枪")] public GameObject sLiteGatling;
    [Tooltip("GM重型火箭炮")] public GameObject GMHeavyRocket;
    [Tooltip("IM冰霜火箭炮")] public GameObject IMFrozenRocket;

    [Header("敌机")] 
    [Tooltip("普通敌机")] 
    public GameObject NormalEnemy;
    public Sprite NormalEnemy2;
    public Sprite NormalEnemy3;
    [Tooltip("中型敌机")] 
    public GameObject ConeEnemy;
    public Sprite ConeEnemy2;
    public Sprite ConeEnemy3;
    [Tooltip("铁皮敌机")]
    public GameObject TinEnemy;
    public Sprite TinEnemy2;
    public Sprite TinEnemy3;
    [Tooltip("闪电敌机")] 
    public GameObject FlashEnemy;
    public Sprite FlashEnemy2;
    public Sprite FlashEnemy3;
    [Tooltip("武装中型敌机")] public GameObject ArmedConeEnemy;
    [Tooltip("武装铁皮敌机")] public GameObject ArmedTinEnemy;

    [Header("Boss")] 
    [Tooltip("FishMaster")] 
    public GameObject FishMaster;
    public Sprite FishMaster2;
    public Sprite FishMaster3;
    public Sprite FishMaster4;


    [Header("子弹")] 
    [Tooltip("子弹")] public GameObject Bullet;
    [Tooltip("子弹爆炸")] public RuntimeAnimatorController BulletBoom;
    [Tooltip("冰霜子弹")] public GameObject FrozenBullet;
    [Tooltip("冰霜子弹爆炸")] public RuntimeAnimatorController FrozenBulletBoom;
    [Tooltip("GM重型火箭炮子弹")] public GameObject GMHeavyRocketBullet;    
    [Tooltip("IM冰霜火箭炮子弹")] public GameObject IMFrozenRocketBullet;
    [Tooltip("守卫者子弹")] public GameObject CattailBullet;
    [Tooltip("s型小型机枪子弹")] public GameObject sLiteGatlingBullet;
    [Tooltip("敌机子弹")] public GameObject EnemyBullet;
    [Tooltip("敌机子弹图片")] public Sprite EnemyBulletImg;
    [Tooltip("敌机子弹爆炸")] public RuntimeAnimatorController EnemyBulletBoom;
    [Tooltip("敌机Killer巡航导弹")] public GameObject KillerModelL;
    [Tooltip("305mm炮弹")] public GameObject EnemyGun305mmBullet;

    public static Rect PlayerMoveRect = Rect.MinMaxRect(-7.72f, -6.7f, 9, 5.34967f);
    public const float DeadlineY = -7f;
    public static Rect BulletAvailableRect = Rect.MinMaxRect(-11.77f, -7f, 11.77f, 9.5f);
}
