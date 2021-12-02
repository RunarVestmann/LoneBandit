using System;
using TMPro;
using UnityEngine;

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
    
    float elapsedTime = 0f;
    bool levelComplete;
    
    int highScore = 0;

    void Update()
    {
        //sets the local high score for this instance of the game 
        //if (score > highScore)
        //    highScore = score;

        if (levelComplete) return;
        
        var timespan = TimeSpan.FromSeconds(elapsedTime);
        timer.text = $"{timespan.Minutes.ToString()} : {timespan.Seconds.ToString()}.{timespan.Milliseconds.ToString()}";
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
    }

    public void OnJumpInput()
    {
        
    }

    public void onPlayerJump() => jumps++;

    public void onPlayerDash() => dashes++;

    public void onPlayerDeath() => deaths++;

    public void onCoinPickup() => score++;
}