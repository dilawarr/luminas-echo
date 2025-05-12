using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Set slider to current volume when scene starts
        if (volumeSlider != null && MusicManager.Instance != null)
        {
            volumeSlider.value = MusicManager.Instance.volume;
        }

        // Listen for slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(value);
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with actual name if different
    }
}
