using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaptersPanel : MonoBehaviour
{
    public void Chapters2Levels(int chapterNum)
    {
        GameData.TargetChapterNum = chapterNum;
        SceneManager.LoadScene("Scenes/Levels");
    }
}
