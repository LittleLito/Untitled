using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossBarPanel : MonoBehaviour
{
    // 血条部分
    private CanvasGroup _bossBarCanvasGroup;
    private Slider _slider;
    
    // 出场Warning
    private Transform _warning;
    private Transform _bossImage;
    private Transform _bossName;

    public void Init()
    {
        // 血条部分
        _bossBarCanvasGroup = transform.Find("BossBar").GetComponent<CanvasGroup>();
        _slider = transform.Find("BossBar/Slider").GetComponent<Slider>();
        _slider.maxValue = BossManager.Instance.Boss.MaxHealth;
        _slider.value = _slider.maxValue;
        // 血条下面的名字
        transform.Find("BossBar/Name").GetComponent<Text>().text = BossManager.Instance.Boss.BossType.ToString();
        // warning背景颜色
        _warning = transform.Find("BossWarning/WarningImage");
        _warning.GetComponent<Image>().color = BossManager.Instance.Boss.BossWarningColor;
        // warning boss图片
        _bossImage = transform.Find("BossWarning/BossImage");
        _bossImage.GetComponent<Image>().sprite = BossManager.Instance.GetBossByType(BossManager.Instance.Boss.BossType).GetComponent<SpriteRenderer>().sprite;
        _bossName = transform.Find("BossWarning/Name");
        // warning boss名称
        _bossName.GetComponent<Text>().text = BossManager.Instance.Boss.BossType.ToString();
    }
    
    /// <summary>
    /// 显示或隐藏boss面板
    /// </summary>
    /// <param name="visible"></param>
    public void SetVisible(bool visible)
    {
        if (visible)
        {
            gameObject.SetActive(true);
            _bossBarCanvasGroup.DOFade(1, 5);
        }
        else
        {
            gameObject.SetActive(false);
            _bossBarCanvasGroup.DOFade(0, 5);
        }
    }

    /// <summary>
    /// 显示炫酷的警告
    /// </summary>
    /// <param name="action">Warning结束后的操作，为boss战开始</param>
    public void ShowWarning(TweenCallback action)
    {
        _warning.DOLocalMoveX(-300, 2).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _warning.DOLocalMoveX(2100, 2).SetEase(Ease.InSine);
        });
        _bossImage.DOLocalMoveY(0, 2).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _bossImage.DOLocalMoveY(-1500, 2).SetEase(Ease.InSine);
        });
        _bossName.DOLocalMoveY(-347, 2).SetEase(Ease.OutSine).OnComplete(() =>
        {
            _bossName.DOLocalMoveY(1000, 2).SetEase(Ease.InSine).OnComplete(action);
        });

    }

    /// <summary>
    /// 更新boss血条
    /// </summary>
    /// <param name="health"></param>
    public void UpdateBossBarInfo(float health)
    {
        _slider.value = health;
    }
}
