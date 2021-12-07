using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    int coinsCollected;
    TextMeshProUGUI coinUI;
    void Awake()
    {
        coinsCollected = 0;
        coinUI = GetComponent<TextMeshProUGUI>();
    }

    public void OnCoinCollect()
    {
        coinsCollected++;
        coinUI.text = coinsCollected.ToString();
    }

    public void SetText(string text) => coinUI.text = text;
}
