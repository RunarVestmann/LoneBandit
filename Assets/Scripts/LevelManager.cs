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
    [SerializeField] bool insideLevel = true;
    [SerializeField] int totalScore;
    bool loadingLevel;
    CoinUI coinUI;

    float elapsedTime = 0f;
    float elapsedTimeSinceLevelComplete;
    bool levelComplete;

    void Awake()
    {
        coinUI = FindObjectOfType<CoinUI>();
        if (insideLevel)
            totalScore = FindObjectsOfType<Coin>().Length;
        else
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var totalTime = 0f;
            for (var i = 1; i < sceneCount - 1; i++)
            {
                if (PlayerPrefs.HasKey($"Level{i}score")) score += PlayerPrefs.GetInt($"Level{i}score");
                if (PlayerPrefs.HasKey($"Level{i}deaths")) deaths += PlayerPrefs.GetInt($"Level{i}deaths");
                if (PlayerPrefs.HasKey($"Level{i}dashes")) dashes += PlayerPrefs.GetInt($"Level{i}dashes");
                if (PlayerPrefs.HasKey($"Level{i}jumps")) jumps += PlayerPrefs.GetInt($"Level{i}jumps");
                if (PlayerPrefs.HasKey($"Level{i}time")) totalTime += PlayerPrefs.GetFloat($"Level{i}time");
            }

            var timespan = TimeSpan.FromSeconds(totalTime);
            timer.text =
                $"{timespan.Minutes.ToString()} : {timespan.Seconds.ToString()}.{timespan.Milliseconds.ToString()}";
            
            // TODO: maybe just hardcode the number of coins in the entire game (this is wrong)
            totalScore = 24;
            
            UpdateUI();
        }

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

        if (!timer || !insideLevel) return;

        var timespan = TimeSpan.FromSeconds(elapsedTime);
        timer.text =
            $"{timespan.Minutes.ToString()} : {timespan.Seconds.ToString()}.{timespan.Milliseconds.ToString()}";
        elapsedTime += Time.deltaTime;
    }

    public void OnLevelComplete()
    {
        if (insideLevel)
        {
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt($"Level{sceneIndex}score", score);
            PlayerPrefs.SetInt($"Level{sceneIndex}jumps", jumps);
            PlayerPrefs.SetInt($"Level{sceneIndex}dashes", dashes);
            PlayerPrefs.SetInt($"Level{sceneIndex}deaths", deaths);
            PlayerPrefs.SetFloat($"Level{sceneIndex}time", elapsedTime);
        }

        levelComplete = true;
        if (levelCompletionUI)
            levelCompletionUI.SetActive(true);
        
        UpdateUI();
        
        player.ForceIdle();
        player.enabled = false;
    }

    void UpdateUI()
    {
        if (jumpUI)
            jumpUI.text = $"Jumps: {jumps}";

        if (dashUI)
            dashUI.text = $"Dashes: {dashes}";

        if (deathUI)
            deathUI.text = $"Deaths: {deaths}";
 
        if (coinUI)
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