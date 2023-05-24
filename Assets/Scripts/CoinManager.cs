using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int coin;

    private void Start()
    {
        //PlayerPrefs.SetInt("coin", 0);
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    private void Update()
    {
        coinText.text = "" + PlayerPrefs.GetInt("coin", 0);
    }

    public void IncreaseCoin(int amount)
    {
        coin = PlayerPrefs.GetInt("coin");
        coin += amount;
        PlayerPrefs.SetInt("coin", coin);
    }
    public void DecreaseCoin(int amount)
    {
        coin = PlayerPrefs.GetInt("coin");
        if (coin - amount < 0)
            coin = 0;
        else coin -= amount;
        PlayerPrefs.SetInt("coin", coin);
    }

    public void SetCoin(int amount)
    {
        PlayerPrefs.SetInt("coin", amount);
    }

    public int GetCoin()
    {
        return PlayerPrefs.GetInt("coin");
    }
}
