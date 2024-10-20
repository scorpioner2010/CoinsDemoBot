using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGame : MonoBehaviour
{
    private int coinCount = 0;
    public TMP_Text coinText;
    public Coin prefabForSpawn;
    public Transform spawnPoint;
    public Button coinButton;

    void Start()
    {
        //coinButton.onClick.AddListener(OnCoinClick);
        UpdateCoinText();
    }

    void OnCoinClick()
    {
        coinCount++;
        UpdateCoinText();
        SpawnFloatingImage();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnCoinClick();
        }
    }

    void UpdateCoinText()
    {
        coinText.text = coinCount.ToString();
    }

    void SpawnFloatingImage()
    {
        Coin coin = Instantiate(prefabForSpawn, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        coin.Init();
    }


}