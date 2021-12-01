using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Player player;
    bool isMenuActive;

    void Awake() => menu.SetActive(isMenuActive);

    public void Toggle()
    {
        isMenuActive = !isMenuActive;
        menu.SetActive(isMenuActive);
        player.enabled = !isMenuActive;
        Time.timeScale = isMenuActive ? 0f : 1f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}