using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    private Text _process;
    private Text _time;
    private Text _killed;
    private Text _damage;
    private Text _absorbed;
    private Text _healed;
    private Text _score;
    private Text _reward;

    public void Init()
    {
        _process = transform.Find("Bg/Process").GetComponent<Text>();
        
        _time = transform.Find("Bg/Values/Time").GetComponent<Text>();
        _killed = transform.Find("Bg/Values/Killed").GetComponent<Text>();
        _damage = transform.Find("Bg/Values/Damage").GetComponent<Text>();
        _absorbed = transform.Find("Bg/Values/Absorbed").GetComponent<Text>();
        _healed = transform.Find("Bg/Values/Healed").GetComponent<Text>();
        _score = transform.Find("Bg/Values/Score").GetComponent<Text>();
        _reward = transform.Find("Bg/Values/Reward").GetComponent<Text>();
    }

    public void Load()
    {
        _process.text = LevelManager.Instance.WaveNum + "/" + LevelManager.Instance.LevelInfo.MaxWaveNum;
        
        var time = LevelManager.Instance.Time;
        var killed = LevelManager.Instance.Stats.GetStatWithType(StatType.Killed).Value;
        var damage = LevelManager.Instance.Stats.GetStatWithType(StatType.Damage).Value;
        var absorbed = LevelManager.Instance.Stats.GetStatWithType(StatType.Absorbed).Value;
        var healed = LevelManager.Instance.Stats.GetStatWithType(StatType.Healed).Value;
        var score = (LevelManager.Instance.LevelInfo.PassScore + damage - time) * PlayerManager.Instance.Health /
                    PlayerManager.Instance.InitMaxHealth;
        score = EnemyManager.Instance.Enemies.Count > 0 ? 0 : score;
        var reward = Math.Max((int) score / 10, 0);
        
        _time.text = Math.Round(time, 2).ToString(CultureInfo.InvariantCulture);
        _killed.text = killed.ToString(CultureInfo.InvariantCulture);
        _damage.text = damage.ToString(CultureInfo.InvariantCulture);
        _absorbed.text = absorbed.ToString(CultureInfo.InvariantCulture);
        _healed.text = healed.ToString(CultureInfo.InvariantCulture);
        _score.text = Math.Max(Math.Round(score, 2), 0).ToString(CultureInfo.InvariantCulture);
        _reward.text = reward.ToString();

        UserDataOperator.UserData.CoinNum += reward;

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