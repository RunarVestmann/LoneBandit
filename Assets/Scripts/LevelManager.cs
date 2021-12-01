using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] int jumps;
    [SerializeField] int deaths;
    [SerializeField] int dashes;
    float elapsedTime = 0f;

    int highScore = 0;

    private void Update()
    {
        //sets the local high score for this instance of the game 
        //if (score > highScore)
        //    highScore = score;

        elapsedTime += Time.deltaTime;
    }

    public void onPlayerJump() => jumps++;

    public void onPlayerDash() => dashes++;

    public void onPlayerDeath() => deaths++;

    public void onCoinPickup() => score++;
}