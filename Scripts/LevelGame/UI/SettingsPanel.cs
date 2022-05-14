using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPanel : MonoBehaviour
{
    /// <summary>
    ///  控制设置面板的显示与否
    /// </summary>
    /// <param name="visible"></param>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
        
        // 如果显示，意味着游戏暂停
        Time.timeScale = visible ? 0 : 1;
    }

    /// <summary>
    /// 返回首页
    /// </summary>
    public void BackToStart()
    {
        GameData.TargetChapterNum = 0;
        GameData.TargetLevelNum = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Scenes/Start");
    }

    /// <summary>
    /// 返回选关
    /// </summary>
    public void BackToLevels()
    {
        GameData.TargetLevelNum = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("Scenes/Levels");
    }
}
