using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private Renderer doorRenderer;
    [SerializeField] private Color closedColor = new Color(0.36f, 0.26f, 0.13f); // brown
    [SerializeField] private Color openColor = Color.black;

    private bool isOpen = false;

    void Start()
    {
        doorRenderer.material.color = closedColor;
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        // Visual feedback (color change)
        doorRenderer.material.color = openColor;

        // Delay scene load (optional)
        // Invoke(nameof(LoadNextScene), 1f); // wait 1 second before loading
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        doorRenderer.material.color = closedColor;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen) return;

        if (other.CompareTag("Player"))
        {
            // LoadNextScene();
            Invoke(nameof(LoadNextScene), 0.5f);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
