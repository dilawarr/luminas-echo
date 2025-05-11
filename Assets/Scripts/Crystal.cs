// using UnityEngine;

// public class Crystal : MonoBehaviour
// {
//     public enum CrystalState { Inactive, Active }
//     public CrystalState currentState = CrystalState.Inactive;
//     [SerializeField] private Transform model; // Assign in Inspector

//     [SerializeField] private Renderer crystalRenderer;
//     [SerializeField] private Color inactiveColor = Color.gray;
//     [SerializeField] private Color activeColor = Color.white;

//     [Header("Beacon Settings")]
//     public GameObject beaconPrefab;
//     [SerializeField] private Transform beaconOrigin;
//     private GameObject emittedBeacon;

//     [Header("Rotation Settings")]
//     // private int currentYStep = 0;
//     private int currentZStep = 0;
//     // private const int maxYSteps = 3;
//     // private const int maxZSteps = 3;
//     private int maxZSteps;
//     private Beacon activatorBeacon;
//     // [SerializeField] private float yStepAngle = 45f;
//     [SerializeField] private float zStepAngle = 22.5f;

//     void Start()
//     {
//         maxZSteps = Mathf.RoundToInt(360f / zStepAngle);
//         UpdateAppearance();
//         UpdateBeaconOriginRotation();
//     }

//     public void StartActivation(Beacon activator)
//     {
//         Debug.Log("Crystal activated");
//         if (activatorBeacon == null)
//         {
//             activatorBeacon = activator;
//             currentState = CrystalState.Active;
//             EmitBeacon();
//             UpdateAppearance();
//         }
//     }

//     public void StopActivation()
//     {
//         if (activatorBeacon != null)
//         {
//             activatorBeacon = null;
//             currentState = CrystalState.Inactive;
//             if (emittedBeacon != null)
//                 Destroy(emittedBeacon);
//             UpdateAppearance();
//         }
//     }

//     // public void RotateY()
//     // {
//     //     currentYStep = (currentYStep + 1) % maxYSteps;
//     //     transform.Rotate(0, yStepAngle, 0);
//     //     UpdateBeaconOriginRotation();
//     //     UpdateBeaconDirection();
//     // }

//     public void RotateZ()
//     {
//         currentZStep = (currentZStep + 1) % maxZSteps;
//         model.Rotate(0, 0, zStepAngle); // Rotate only the visual part
//         UpdateBeaconOriginRotation();
//         if (emittedBeacon != null)
//         {
//             UpdateBeaconDirection();
//         }

//     }
//     private void UpdateBeaconOriginRotation()
//     {
//         // Quaternion yRotation = Quaternion.Euler(0, currentYStep * yStepAngle, 0);
//         // Quaternion zRotation = Quaternion.Euler(0, 0, currentZStep * zStepAngle);
//         // beaconOrigin.localRotation = yRotation * zRotation;
//         Quaternion zRotation = Quaternion.Euler(0, 0, currentZStep * zStepAngle);
//         beaconOrigin.localRotation = zRotation;
//     }
//     private void EmitBeacon()
//     {
//         if (beaconPrefab != null)
//         {
//             Debug.Log("Emitting beacon...");

//             Vector3 forward = beaconOrigin.right; // Use right if your beacon grows along local X
//             Vector3 spawnPos = beaconOrigin.position + forward * 100f;

//             emittedBeacon = Instantiate(beaconPrefab, spawnPos, beaconOrigin.rotation);

//             Collider beaconCollider = emittedBeacon.GetComponent<Collider>();
//             Collider thisCrystalCollider = GetComponent<Collider>();

//             if (beaconCollider != null && thisCrystalCollider != null)
//             {
//                 Physics.IgnoreCollision(beaconCollider, thisCrystalCollider);
//             }
//         }
//         else
//         {
//             Debug.LogWarning("Beacon prefab is missing!");
//         }
//     }

//     private void LateUpdate()
//     {
//         UpdateBeaconDirection();
//     }

//     // private void EmitBeacon()
//     // {
//     //     if (beaconPrefab != null)
//     //     {
//     //         Vector3 spawnPosition = beaconOrigin.position;
//     //         Quaternion spawnRotation = beaconOrigin.rotation;

//     //         // 🔄 Example: override direction manually (e.g., shoot along X)
//     //         spawnRotation = Quaternion.LookRotation(Vector3.right);

//     //         emittedBeacon = Instantiate(beaconPrefab, spawnPosition, spawnRotation);
//     //     }
//     // }

//     private void UpdateBeaconDirection()
//     {
//         if (emittedBeacon == null) return;

//         Beacon beacon = emittedBeacon.GetComponent<Beacon>();
//         // if (emittedBeacon != null && currentState == CrystalState.Active)
//         if (beacon != null)
//         {
//             beacon.ResetGrowth(beaconOrigin.position, beaconOrigin.rotation);
//         }
//     }

//     private void UpdateAppearance()
//     {
//         if (crystalRenderer != null)
//         {
//             crystalRenderer.material.color = (currentState == CrystalState.Active) ? activeColor : inactiveColor;
//         }
//     }
// }

using UnityEngine;

public class Crystal : MonoBehaviour
{
    public enum CrystalState { Inactive, Active }
    public CrystalState currentState = CrystalState.Inactive;
    [SerializeField] private Transform model; // Assign in Inspector

    [SerializeField] private Renderer crystalRenderer;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color activeColor = Color.white;

    [Header("Beacon Settings")]
    public GameObject beaconPrefab;
    [SerializeField] private Transform beaconOrigin;
    private GameObject emittedBeacon;

    [Header("Rotation Settings")]
    private int currentZStep = 0;
    private int maxZSteps;
    private Beacon activatorBeacon;
    [SerializeField] private float zStepAngle = 22.5f;
    
    [Header("Debug")]
    [SerializeField] private bool drawDebugRays = true;
    [SerializeField] private float beaconOffset = 1.0f; // Increased offset

    void Start()
    {
        maxZSteps = Mathf.RoundToInt(360f / zStepAngle);
        UpdateAppearance();
        UpdateBeaconOriginRotation();
    }

    public void StartActivation(Beacon activator)
    {
        Debug.Log("Crystal activated");
        if (activatorBeacon == null)
        {
            activatorBeacon = activator;
            currentState = CrystalState.Active;
            EmitBeacon();
            UpdateAppearance();
        }
    }

    public void StopActivation()
    {
        if (activatorBeacon != null)
        {
            activatorBeacon = null;
            currentState = CrystalState.Inactive;
            if (emittedBeacon != null)
                Destroy(emittedBeacon);
            UpdateAppearance();
        }
    }

    public void RotateZ()
    {
        currentZStep = (currentZStep + 1) % maxZSteps;
        model.Rotate(0, 0, zStepAngle); // Rotate only the visual part
        UpdateBeaconOriginRotation();
        if (emittedBeacon != null)
        {
            UpdateBeaconDirection();
        }
    }

    private void UpdateBeaconOriginRotation()
    {
        Quaternion zRotation = Quaternion.Euler(0, 0, currentZStep * zStepAngle);
        beaconOrigin.localRotation = zRotation;
    }

    private void EmitBeacon()
    {
        if (beaconPrefab != null)
        {
            Debug.Log("Emitting beacon...");

            // Calculate the spawn position with a larger offset to escape crystal collider
            Vector3 forward = beaconOrigin.right; // Use right if your beacon grows along local X
            Vector3 spawnPos = beaconOrigin.position + forward * beaconOffset;

            // Draw debug ray to visualize where the beacon should be spawning
            if (drawDebugRays)
            {
                Debug.DrawRay(beaconOrigin.position, forward * 5f, Color.red, 5f);
                Debug.DrawLine(beaconOrigin.position, spawnPos, Color.yellow, 5f);
            }

            // Destroy any existing beacon first
            if (emittedBeacon != null)
            {
                Destroy(emittedBeacon);
            }

            // Create the new beacon with the proper offset
            emittedBeacon = Instantiate(beaconPrefab, spawnPos, beaconOrigin.rotation);
            
            // Set the parent crystal reference
            Beacon beaconComponent = emittedBeacon.GetComponent<Beacon>();
            if (beaconComponent != null)
            {
                beaconComponent.SetParentCrystal(this);
            }
            
            // Explicitly ignore collisions between crystal and emitted beacon
            Collider beaconCollider = emittedBeacon.GetComponent<Collider>();
            Collider crystalCollider = GetComponent<Collider>();
            if (beaconCollider != null && crystalCollider != null)
            {
                Physics.IgnoreCollision(beaconCollider, crystalCollider, true);
                Debug.Log("Explicitly ignoring collision between beacon and crystal");
            }
        }
        else
        {
            Debug.LogWarning("Beacon prefab is missing!");
        }
    }

    private void OnDrawGizmos()
    {
        if (drawDebugRays && beaconOrigin != null)
        {
            // Draw a sphere at the beacon origin
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(beaconOrigin.position, 0.2f);
            
            // Draw a line indicating the direction and offset
            Vector3 forward = beaconOrigin.right;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(beaconOrigin.position, beaconOrigin.position + forward * beaconOffset);
        }
    }

    private void LateUpdate()
    {
        UpdateBeaconDirection();
    }

    private void UpdateBeaconDirection()
    {
        if (emittedBeacon == null) return;

        Beacon beacon = emittedBeacon.GetComponent<Beacon>();
        if (beacon != null)
        {
            // Calculate the new position with offset
            Vector3 forward = beaconOrigin.right;
            Vector3 offsetPosition = beaconOrigin.position + forward * beaconOffset;
            
            // Update the beacon position and rotation
            beacon.ResetGrowth(offsetPosition, beaconOrigin.rotation);
        }
    }

    private void UpdateAppearance()
    {
        if (crystalRenderer != null)
        {
            crystalRenderer.material.color = (currentState == CrystalState.Active) ? activeColor : inactiveColor;
        }
    }
}