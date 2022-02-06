using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // 配置资源
    public GameConfig GameConfig { get; private set; }
    
    // 关卡数
    public int LevelNum;

    

    private void Awake()
    {
        Instance = this;
        GameConfig = Resources.Load<GameConfig>("GameConfig"); 

    }

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.Instance.StartLevel(LevelNum);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
