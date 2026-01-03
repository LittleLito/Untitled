using UnityEngine;
using UnityEngine.EventSystems;

public class UIShowCard : UICard, IPointerClickHandler
{
    public EquipType Type;
    public override EquipType EquipType => Type;
    public bool IsChosen;

    /// <summary>
    /// 点击时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (LevelManager.Instance.LevelState != LevelState.Selecting) return;
        
        if (!IsChosen)
        {
            IsChosen = true;
            UIManager.Instance.AddChosenCard(gameObject);
        }
        else
        {
            IsChosen = false;
            UIManager.Instance.RemoveChosenCard(gameObject);
        }
    }

    /// <summary>
    /// 根据索引更新在仓库中的位置
    /// </summary>
    public void UpdatePositionInStorage()
    {
        _rectTransform.anchorMax = new Vector2(0, 1);
        _rectTransform.anchorMin = new Vector2(0, 1);
        _rectTransform.pivot = new Vector2(0, 1);
        transform.position = new Vector3(((int)EquipType - 1) % 8 * 119 + 16, - (((int)EquipType - 1) / 8 * 53 + 16), 0) + transform.parent.position;

        if (LevelManager.Instance.LevelInfo.IsNight || !(_equipScript is IMoonEnergyEquip)) return;
        _maskImg.fillAmount = 1;
        _cardImg.color = new Color(0.75f, 0.75f, 0.75f);

    }
}