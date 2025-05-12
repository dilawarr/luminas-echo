using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private Animator doorAnimator;

    private bool isOpen = false;

    private void Start()
    {
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
    }

    public void Open()
    {
        if (isOpen) return;

        Debug.Log("Opened door");
        isOpen = true;
        doorAnimator.SetTrigger("OpenDoor");
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        doorAnimator.SetTrigger("CloseDoor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen || !other.CompareTag("Player")) return;

        Invoke(nameof(LoadNextScene), 0.5f);
    }

    private void LoadNextScene()
    {
        GameManager.Instance.LoadSceneByIndex(nextSceneIndex);
    }
}
