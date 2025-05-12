using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(Constants.LevelOne);
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(Constants.Options);
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene(Constants.LevelSelector);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
