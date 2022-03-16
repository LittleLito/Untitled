using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChaptersPanel : MonoBehaviour
{
    private Text _coinNum;

    private void Start()
    {
        _coinNum = transform.Find("Coin/Num").GetComponent<Text>();
    }

    private void Update()
    {
        _coinNum.text = UserDataOperator.UserData.CoinNum.ToString();
    }

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
