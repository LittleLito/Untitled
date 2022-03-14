using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RecoverFruitManager : MonoBehaviour
{
    public static RecoverFruitManager Instance;
    
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
            CreateFruit();
        }
    }

    /// <summary>
    /// 开始生成能量
    /// </summary>
    public void StartCreate()
    {
        var time = Random.Range(180, 300);
        Invoke(nameof(CreateFruit), time);
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
    /// 生成回复果
    /// </summary>
    private void CreateFruit()
    {
        _canCreate = false;  // 
        
        // 生成并获取回复果对象
        var fruit = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.RecoverFruit, transform)
            .GetComponent<RecoverFruit>();
        fruit.Init();

        Invoke(nameof(SetCanCreateTrue), Random.Range(50, 70));  // 计时开始
    }

    /// <summary>
    /// 计时结束后设置为可生成
    /// </summary>
    private void SetCanCreateTrue()
    {
        _canCreate = true;
    }
}
