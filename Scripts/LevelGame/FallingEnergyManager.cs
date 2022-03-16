using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingEnergyManager : MonoBehaviour
{
    public static FallingEnergyManager Instance;
    
    // 位置参数
    private const float SpawnY = 10;
    private const float SpawnMinX = -7.03f;
    private const float SpawnMaxX = 8.43f;
    private const float StopMinY = -6.25f;
    private const float StopMaxY = -1.89f;
    // 能否生成
    private bool _canCreate;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_canCreate)
        {
            CreateEnergy();
        }
    }

    /// <summary>
    /// 开始生成能量
    /// </summary>
    public void StartCreate()
    {
        var time = Random.Range(6f, 10f);
        Invoke(nameof(CreateEnergy), time);
    }

    /// <summary>
    /// 生成结束(可再次开启)
    /// </summary>
    public void StopCreate()
    {
        CancelInvoke();
        _canCreate = false;
    }


    /// <summary>
    /// 生成能量
    /// </summary>
    private void CreateEnergy()
    {
        _canCreate = false;  // 
        
        // 生成并获取能量球对象
        var energy = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Energy, transform).GetComponent<Energy>();
        var fallingDownY = Random.Range(StopMinY, StopMaxY);
        var spawnX = Random.Range(SpawnMinX, SpawnMaxX);
        energy.InitForSky(fallingDownY, new Vector2(spawnX, SpawnY));  // 初始化位置

        Invoke(nameof(SetCanCreateTrue), Random.Range(14f, 18f));  // 计时开始
    }

    /// <summary>
    /// 计时结束后设置为可生成
    /// </summary>
    private void SetCanCreateTrue()
    {
        _canCreate = true;
    }
}
