using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public TextMeshProUGUI coin;
    private void Update()
    {
        coin.text = UserDataOperator.UserData.CoinNum.ToString();
    }
}
