using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : MonoBehaviour
{
    private LevelInfo _levelInfo;
    private GameObject _levelPanel;
    private Text _levelNum;
    private Text _maxWaveNum;
    private Text _difficulty;
    private Text _reward;
    private void Start()
    {
        _levelInfo = GameData.GetLevelInfo();
        
        _levelPanel = transform.Find("LevelPanel").gameObject;
        _levelPanel.SetActive(false);
        _levelNum = transform.Find("LevelPanel/LevelNum").GetComponent<Text>();
        _maxWaveNum = transform.Find("LevelPanel/Canvas/Part2/MaxWaveNum").GetComponent<Text>();
        _difficulty = transform.Find("LevelPanel/Canvas/Part2/Difficulty").GetComponent<Text>();
        _reward = transform.Find("LevelPanel/Canvas/Part2/Reward").GetComponent<Text>();
    }

    public void Levels2Chapters()
    {
        GameData.TargetChapterNum = 0;
        SceneManager.LoadScene("Scenes/Chapters");
    }

    public void ShowLevelPanel(int levelNum)
    {
        GameData.TargetLevelNum = levelNum;

        var levelInfo = GameData.GetLevelInfo();
        _levelNum.text = "关卡" + GameData.TargetChapterNum + "-" + GameData.TargetLevelNum;
        _maxWaveNum.text = levelInfo.MaxWaveNum.ToString();
        _difficulty.text = 1.ToString();
        _reward.text = 500 + "g";
        
        _levelPanel.SetActive(true);

    }

    public void Level2Game()
    {
        SceneManager.LoadScene("Scenes/LevelGame");
    }

    public void SetLevelPanelInactive()
    {
        GameData.TargetLevelNum = 0;
        _levelPanel.SetActive(false);
    }
}
