using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIShowCard : UICard, IPointerClickHandler
{
    public EquipType Type;
    public override EquipType EquipType => Type;
    public bool IsChosen;
    public int Index;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 点击时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
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
        GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        transform.position = new Vector3((Index % 8) * 119 + 16, - ((Index / 8) * 53 + 16), 0) + transform.parent.position;
    }
}