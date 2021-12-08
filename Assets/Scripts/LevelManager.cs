using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int score;
    public int jumps;
    public int deaths;
    public int dashes;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] GameObject levelCompletionUI;
    [SerializeField] TextMeshProUGUI jumpUI;
    [SerializeField] TextMeshProUGUI dashUI;
    [SerializeField] TextMeshProUGUI deathUI;
    [SerializeField] Player player;
    [SerializeField] float timeUntilYouCanChangeLevels = 1.3f;
    bool loadingLevel;
    CoinUI coinUI;
    int totalScore;

    float elapsedTime = 0f;
    float elapsedTimeSinceLevelComplete;
    bool levelComplete;

    void Awake()
    {
        coinUI = FindObjectOfType<CoinUI>();
        totalScore = FindObjectsOfType<Coin>().Length;
        if (PlayerPrefs.HasKey("mainVolume"))
            AudioListener.volume = PlayerPrefs.GetFloat("mainVolume");
    }

    void Update()
    {
        if (levelComplete)
        {
            elapsedTimeSinceLevelComplete += Time.deltaTime;
            return;
        }

        var timespan = TimeSpan.FromSeconds(elapsedTime);
        timer.text =
            $"{timespan.Minutes.ToString()} : {timespan.Seconds.ToString()}.{timespan.Milliseconds.ToString()}";
        elapsedTime += Time.deltaTime;
    }

    public void OnLevelComplete()
    {
        levelComplete = true;
        levelCompletionUI.SetActive(true);
        jumpUI.text = $"Jumps: {jumps}";
        dashUI.text = $"Dashes: {dashes}";
        deathUI.text = $"Deaths: {deaths}";
        player.ForceIdle();
        player.enabled = false;
        coinUI.SetText($"{score}/{totalScore}");
    }

    public void OnJumpInput()
    {
        if (!levelComplete || loadingLevel || timeUntilYouCanChangeLevels > elapsedTimeSinceLevelComplete) return;
        loadingLevel = true;
        var index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(index < SceneManager.sceneCountInBuildSettings ? index : 0);
    }

    public void onPlayerJump() => jumps++;

    public void onPlayerDash() => dashes++;

    public void onPlayerDeath() => deaths++;

    public void onCoinPickup() => score++;
}