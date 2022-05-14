using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsPanel : MonoBehaviour
{
    public GameObject levelPanel;
    public TMP_Text levelNum;
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
}