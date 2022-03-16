using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // 配置资源
    public GameConfig GameConfig { get; private set; }


    private void Awake()
    {
        Instance = this;
        GameConfig = Resources.Load<GameConfig>("GameConfig");

    }

    private void Start()
    {
        LevelManager.Instance.StartLevel(GameData.GetLevelInfo());
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
