using System;
using System.Collections;
using System.Collections.Generic;
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
    public void BackToFrontPage()
    {
        SceneManager.LoadScene("Scenes/Start");
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
