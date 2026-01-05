using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : MonoBehaviour
{
    public GameObject levelPanel;
    public TMP_Text levelNum;
    public RawImage levelImage;
    public List<Texture> levelImages;
    public TMP_Text maxWaveNum;
    public TMP_Text difficulty;
    public TMP_Text passScore;


    private void Start()
    {
        levelPanel.SetActive(false);
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
        this.levelNum.text = "关卡" + GameData.TargetChapterNum + "-" + GameData.TargetLevelNum;
        levelImage.texture = levelImages[GameData.TargetChapterNum - 1];
        maxWaveNum.text = levelInfo.MaxWaveNum.ToString();
        difficulty.text = 1.ToString();
        passScore.text = levelInfo.PassScore.ToString();

        levelPanel.SetActive(true);
    }
    
    public void SetLevelPanelInactive()
    {
        GameData.TargetLevelNum = 0;
        levelPanel.SetActive(false);
    }

    public void LevelPanel2LevelGame()
    {
        if (GameData.IsLevelValid())
        {
            SceneManager.LoadScene("Scenes/LevelGame");
        }
    }
}