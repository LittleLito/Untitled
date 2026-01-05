using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void Start2Chapters()
    {
        SceneManager.LoadScene("Scenes/Chapters");
    }
    
    public void Start2Almanac()
    {
        SceneManager.LoadScene("Scenes/Almanac");
    }

    public void Quit()
    {
        UserDataOperator.SaveUserData();
        Application.Quit();
    }
}
