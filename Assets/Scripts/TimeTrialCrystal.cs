using UnityEngine;

public class TimeTrialCrystal : MonoBehaviour
{
    public enum State { Inactive, Active }
    private State state = State.Inactive;

    [SerializeField] private Renderer crystalRenderer;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color activeColor = Color.cyan;

    [Header("Ghost Platforms")]
    [SerializeField] private GameObject[] ghostPlatforms;
    [SerializeField] private float activeDuration = 5f;

    private Beacon incomingBeacon;
    private float timer;

    private void Start()
    {
        UpdateVisual();
        DeactivatePlatforms();
    }

    private void Update()
    {
        if (state == State.Active)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                state = State.Inactive;
                incomingBeacon = null;

                UpdateVisual();
                DeactivatePlatforms();
            }
        }
    }

    public void Activate(Beacon source)
    {
        if (state == State.Inactive)
        {
            state = State.Active;
            UpdateVisual();
            ActivatePlatforms();
        }

        // Always reset timer and track the new beacon (if needed)
        incomingBeacon = source;
        timer = activeDuration;
    }

    public void Deactivate(Beacon source)
    {
        if (incomingBeacon == source)
        {
            incomingBeacon = null;
            state = State.Inactive;
            UpdateVisual();
            DeactivatePlatforms();
        }
    }

    private void ActivatePlatforms()
    {
        foreach (GameObject platform in ghostPlatforms)
        {
            platform.SetActive(true);
        }
    }

    private void DeactivatePlatforms()
    {
        foreach (GameObject platform in ghostPlatforms)
        {
            platform.SetActive(false);
        }
    }

    private void UpdateVisual()
    {
        if (crystalRenderer != null)
        {
            crystalRenderer.material.color = (state == State.Active) ? activeColor : inactiveColor;
        }
    }
}
