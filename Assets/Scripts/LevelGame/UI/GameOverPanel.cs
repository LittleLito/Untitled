using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    private TMP_Text _process;
    private TMP_Text _time;
    private TMP_Text _killed;
    private TMP_Text _damage;
    private TMP_Text _absorbed;
    private TMP_Text _healed;
    private TMP_Text _score;
    private TMP_Text _reward;

    public void Init()
    {
        _process = transform.Find("Bg/Process").GetComponent<TMP_Text>();
        
        _time = transform.Find("Bg/Values/Time").GetComponent<TMP_Text>();
        _killed = transform.Find("Bg/Values/Killed").GetComponent<TMP_Text>();
        _damage = transform.Find("Bg/Values/Damage").GetComponent<TMP_Text>();
        _absorbed = transform.Find("Bg/Values/Absorbed").GetComponent<TMP_Text>();
        _healed = transform.Find("Bg/Values/Healed").GetComponent<TMP_Text>();
        _score = transform.Find("Bg/Values/Score").GetComponent<TMP_Text>();
        _reward = transform.Find("Bg/Values/Reward").GetComponent<TMP_Text>();
    }

    public void Load()
    {
        _process.text = LevelManager.Instance.WaveNum + "/" + LevelManager.Instance.LevelInfo.MaxWaveNum;
        
        var time = LevelManager.Instance.Time;
        var killed = LevelManager.Instance.Stats.GetStatWithType(StatType.Killed).Value;
        var damage = LevelManager.Instance.Stats.GetStatWithType(StatType.Damage).Value;
        var absorbed = LevelManager.Instance.Stats.GetStatWithType(StatType.Absorbed).Value;
        var healed = LevelManager.Instance.Stats.GetStatWithType(StatType.Healed).Value;
        var score = (LevelManager.Instance.LevelInfo.PassScore + damage / 5f - time) * PlayerManager.Instance.Health /
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