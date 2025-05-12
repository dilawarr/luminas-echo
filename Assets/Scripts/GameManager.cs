using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Uncomment if GameManager should persist across scenes
        // DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentScene(float delay = 0f)
    {
        if (delay > 0f)
        {
            Invoke(nameof(DoRestart), delay);
        }
        else
        {
            DoRestart();
        }
    }

    private void DoRestart()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}
