using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    public void Start2Adventure()
    {
        SceneManager.LoadScene("LevelGame");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
