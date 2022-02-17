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
    //public LevelInfo LevelInfo { get; private set; }


    private void Awake()
    {
        Instance = this;
        GameConfig = Resources.Load<GameConfig>("GameConfig");

        // var jsonStr = Resources.Load<TextAsset>("LevelInfo");
        // var allLevelsInfo = JsonUtility.FromJson<AllLevelsInfo>(jsonStr.text);
        // LevelInfo = allLevelsInfo.Levels.Find(info =>
        //     info.Num == (GameData.TargetChapterNum - 1) * 10 + GameData.TargetLevelNum);
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
