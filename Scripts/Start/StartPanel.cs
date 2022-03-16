using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
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

    public void Start2Adventure()
    {
        SceneManager.LoadScene("Scenes/Chapters");
    }

    public void Quit()
    {
        UserDataOperator.SaveUserData();
        Application.Quit();
    }
}
