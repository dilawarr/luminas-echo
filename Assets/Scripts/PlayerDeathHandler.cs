using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 1.5f;
    private bool isRespawning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isRespawning) return;

        if (other.CompareTag("Death"))
        {
            isRespawning = true;

            // Optionally disable movement script here
            // GetComponent<PlayerMovement>().enabled = false;

            GameManager.Instance.RestartCurrentScene(respawnDelay);
        }
    }
}
