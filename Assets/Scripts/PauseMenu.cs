using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] Player player;
    [SerializeField] Light2D globalLight;
    [SerializeField] float pauseLightIntensity;
    bool isMenuActive;
    float defaultLightIntensity;

    void Awake()
    {
        menu.SetActive(isMenuActive);
        defaultLightIntensity = globalLight.intensity;
    }

    public void Toggle()
    {
        isMenuActive = !isMenuActive;
        menu.SetActive(isMenuActive);
        globalLight.intensity = isMenuActive ? pauseLightIntensity : defaultLightIntensity;
        player.enabled = !isMenuActive;
        Time.timeScale = isMenuActive ? 0f : 1f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}