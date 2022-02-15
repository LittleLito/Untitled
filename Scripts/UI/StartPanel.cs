using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    private void Start()
    {
        GameData.InitLevelInfos();
    }

    public void Start2Adventure()
    {
        SceneManager.LoadScene("Scenes/Chapters");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
