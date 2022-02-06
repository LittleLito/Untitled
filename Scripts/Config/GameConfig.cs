using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

/// <summary>
/// 游戏配置
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("卡片相关")]
    [Tooltip("卡片")] public GameObject Card;
    [Tooltip("普通卡片")] public Sprite CardCommon;
    [Tooltip("能量生产卡片")] public Sprite CardEnlighten;

    [Header("其他资源")] 
    [Tooltip("AGENCYB")] public Font AGENCYB;
    [Tooltip("LXGWWENKAI-BOLD")] public Font LXGWWENKAI_BOLD;
    [Tooltip("装备位置管理")] public GameObject LocationManager;
    [Tooltip("能量")] public GameObject Energy;
    [Tooltip("爆炸效果")] public RuntimeAnimatorController Explosion;
    [Tooltip("喷射尾气")] public RuntimeAnimatorController Gas;

    [Header("装备")]
    [Tooltip("S型轻型机枪")] public GameObject SGatling;
    [Tooltip("能源反应堆")] public GameObject EnergyReactor;

    [Header("敌机")] 
    [Tooltip("普通敌机")] 
    public GameObject NormalEnemy;
    public Sprite NormalEnemy1;
    public Sprite NormalEnemy2;
    public Sprite NormalEnemy3;
    [Tooltip("中型敌机")] 
    public GameObject ConeEnemy;
    public Sprite ConeEnemy1;
    public Sprite ConeEnemy2;
    public Sprite ConeEnemy3;
    [Tooltip("铁皮敌机")]
    public GameObject TinEnemy;
    public Sprite TinEnemy1;
    public Sprite TinEnemy2;
    public Sprite TinEnemy3;

    [Header("子弹")] 
    [Tooltip("子弹")] public GameObject Bullet;
    [Tooltip("子弹图片")] public Sprite BulletImg;
    [Tooltip("枪口火焰")] public Sprite Gunfire;
    [Tooltip("子弹爆炸")] public RuntimeAnimatorController BulletBoom;
}