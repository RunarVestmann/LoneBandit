using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void LoadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadScene(Scene scene) => SceneManager.LoadScene(scene.buildIndex);
}