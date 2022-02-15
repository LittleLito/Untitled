using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaptersPanel : MonoBehaviour
{
    public void Chapters2Start()
    {
        SceneManager.LoadScene("Scenes/Start");
    }

    public void Chapters2Levels(int chapterNum)
    {
        GameData.TargetChapterNum = chapterNum;
        SceneManager.LoadScene("Scenes/Levels");
    }
}
