using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] int jumps;
    [SerializeField] int deaths;
    [SerializeField] int dashes;

    int highScore = 0;

    private void Start()
    {
    }
    private void Update()
    {
        //sets the local high score for this instance of the game 
        //if (score > highScore)
        //    highScore = score;
    }
    public void onPlayerJump() { jumps++; }
    public void onPlayerDash() { dashes++; }
    public void onPlayerDeath() { deaths++; }
    public void onCoinPickup() { score++; }
}
