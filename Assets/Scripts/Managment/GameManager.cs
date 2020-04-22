using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private MenuController menuController;
    private int countCoin;
    private int rewards;

    public int GetCountCoin => countCoin;
    public int GetCountReward => rewards;

    private void OnEnable()
    {
        EventManager.OnCoinTake += OnCoinTake;
        EventManager.OnManFinish += EndLevel;
        EventManager.OnRestartLevel += OnLevelRestart;
    }

    private void OnDisable()
    {
        EventManager.OnCoinTake -= OnCoinTake;
        EventManager.OnManFinish -= EndLevel;
        EventManager.OnRestartLevel -= OnLevelRestart;
    }

    private void OnLevelRestart()
    {
        rewards = 0;
        EventManager.OnCoinTextUpdate?.Invoke(rewards);
    }

    private void EndLevel()
    {
        countCoin += rewards;
        SaveData();
        menuController.ShowEndLevelMenu();
    }

    private void OnCoinTake()
    {
        rewards++;
        EventManager.OnCoinTextUpdate?.Invoke(rewards);
    }

    private void LoadData()
    {
        countCoin = PlayerPrefs.GetInt(GameConstants.PlayerCoinKey, 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(GameConstants.PlayerCoinKey, countCoin);
    }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadData();
        EventManager.OnCoinTextUpdate?.Invoke(rewards);
    }
}
