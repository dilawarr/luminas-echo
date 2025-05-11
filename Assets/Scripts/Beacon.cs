// using UnityEngine;

// [RequireComponent(typeof(LineRenderer), typeof(BoxCollider))]
// public class Beacon : MonoBehaviour
// {
//     [SerializeField] private float maxDistance = 50f;
//     [SerializeField] private float growSpeed = 20f; // Units per second

//     private LineRenderer line;
//     private BoxCollider boxCollider;
//     private float currentLength = 0f;
//     private bool growing = true;

//     private void Start()
//     {
//         line = GetComponent<LineRenderer>();
//         boxCollider = GetComponent<BoxCollider>();

//         line.positionCount = 2;
//         line.useWorldSpace = true;

//         boxCollider.isTrigger = true;
//         UpdateBeamVisuals();
//     }

//     private void Update()
//     {
//         if (!growing) return;

//         currentLength += growSpeed * Time.deltaTime;
//         currentLength = Mathf.Min(currentLength, maxDistance);
//         UpdateBeamVisuals();
//     }

//     private void UpdateBeamVisuals()
//     {
//         Vector3 origin = transform.position;
//         Vector3 direction = transform.right;
//         Vector3 endPoint = origin + direction * currentLength;

//         line.SetPosition(0, origin);
//         line.SetPosition(1, endPoint);

//         boxCollider.center = new Vector3(currentLength / 2f, 0, 0);
//         boxCollider.size = new Vector3(currentLength, 0.1f, 0.1f);
//     }

// private void OnTriggerEnter(Collider other)
// {
//     // Debug.Log($"Beacon hit: {other.name} | Tag: {other.tag}");

//     // Use root to be safe
//     if (other.transform.root.CompareTag("Crystal"))
//     {
//         Crystal hitCrystal = other.transform.root.GetComponent<Crystal>();
//         if (hitCrystal != null)
//         {
//             hitCrystal.StartActivation(this);
//         }

//         growing = false;
//     }

//     else if (other.CompareTag("Door"))
//     {
//         Door door = other.GetComponent<Door>();
//         if (door != null)
//         {
//             door.Open();
//         }

//         growing = false;
//     }

//     else if (other.CompareTag("Wall") || other.CompareTag("Obstacle") || other.CompareTag("Ground"))
//     {
//         growing = false;
//     }
// }

//     public void ResetGrowth(Vector3 newPosition, Quaternion newRotation)
//     {
//         transform.position = newPosition;
//         transform.rotation = newRotation;
//         currentLength = 0f;
//         growing = true;
//     }


//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("Crystal"))
//         {
//             Crystal crystal = other.GetComponent<Crystal>();
//             if (crystal != null)
//             {
//                 crystal.StopActivation();
//             }
//         }

//         if (other.CompareTag("Door"))
//         {
//             Door door = other.GetComponent<Door>();
//             if (door != null)
//             {
//                 door.Close();
//             }
//         }

//     }

//     private void OnDisable()
//     {
//         growing = false;
//     }
// }

using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(BoxCollider))]
public class Beacon : MonoBehaviour
{
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float growSpeed = 20f; // Units per second
    [SerializeField] private bool drawDebugInfo = true;

    private LineRenderer line;
    private BoxCollider boxCollider;
    private float currentLength = 0f;
    private bool growing = true;
    
    // Store the parent crystal that created this beacon
    private Crystal parentCrystal;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        line.positionCount = 2;
        line.useWorldSpace = true;

        boxCollider.isTrigger = true;
        
        // Start with zero length
        currentLength = 0f;
        UpdateBeamVisuals();
        
        if (drawDebugInfo)
        {
            Debug.Log($"Beacon started at position: {transform.position}");
        }
    }

    private void Update()
    {
        if (!growing) return;

        currentLength += growSpeed * Time.deltaTime;
        currentLength = Mathf.Min(currentLength, maxDistance);
        UpdateBeamVisuals();
    }

    private void UpdateBeamVisuals()
    {
        // Set the beam's start point at the current position
        Vector3 origin = transform.position;
        
        // The beam grows along the local right axis
        Vector3 direction = transform.right;
        
        // Calculate end point based on current length
        Vector3 endPoint = origin + direction * currentLength;

        // Update the line renderer
        line.SetPosition(0, origin);
        line.SetPosition(1, endPoint);

        // Update the collider to match the beam
        boxCollider.center = new Vector3(currentLength / 2f, 0, 0);
        boxCollider.size = new Vector3(currentLength, 0.1f, 0.1f);
        
        if (drawDebugInfo && parentCrystal != null)
        {
            Debug.DrawLine(origin, origin + Vector3.up * 0.5f, Color.magenta, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (drawDebugInfo)
        {
            Debug.Log($"Beacon collision with: {other.name} | Tag: {other.tag}");
        }
        
        // Skip collision with parent crystal
        Crystal hitCrystal = other.transform.root.GetComponent<Crystal>();
        if (hitCrystal != null && hitCrystal == parentCrystal)
        {
            if (drawDebugInfo)
            {
                Debug.Log("Ignoring collision with parent crystal");
            }
            return; // Ignore collision with parent crystal
        }

        // Handle collision with other crystals
        if (other.transform.root.CompareTag("Crystal"))
        {
            if (hitCrystal != null)
            {
                hitCrystal.StartActivation(this);
                if (drawDebugInfo)
                {
                    Debug.Log($"Activated crystal: {hitCrystal.name}");
                }
            }
            growing = false;
        }
        else if (other.CompareTag("Door"))
        {
            Door door = other.GetComponent<Door>();
            if (door != null)
            {
                door.Open();
            }
            growing = false;
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Obstacle") || other.CompareTag("Ground"))
        {
            growing = false;
            if (drawDebugInfo)
            {
                Debug.Log($"Beacon stopped growing after hitting: {other.tag}");
            }
        }
    }

    public void ResetGrowth(Vector3 newPosition, Quaternion newRotation)
    {
        // Update the position and rotation
        transform.position = newPosition;
        transform.rotation = newRotation;
        
        // Reset the beam
        currentLength = 0f;
        growing = true;
        
        // Update the visual immediately
        UpdateBeamVisuals();
    }

    // Set the parent crystal reference
    public void SetParentCrystal(Crystal crystal)
    {
        parentCrystal = crystal;
        
        // Just to be extra safe, explicitly ignore collisions with parent
        if (crystal != null)
        {
            Collider thisCollider = GetComponent<Collider>();
            Collider crystalCollider = crystal.GetComponent<Collider>();
            
            if (thisCollider != null && crystalCollider != null)
            {
                Physics.IgnoreCollision(thisCollider, crystalCollider, true);
                if (drawDebugInfo)
                {
                    Debug.Log($"Beacon explicitly ignoring collisions with parent crystal: {crystal.name}");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            Crystal crystal = other.GetComponent<Crystal>();
            if (crystal != null && crystal != parentCrystal) // Don't deactivate the parent
            {
                crystal.StopActivation();
            }
        }

        if (other.CompareTag("Door"))
        {
            Door door = other.GetComponent<Door>();
            if (door != null)
            {
                door.Close();
            }
        }
    }

    private void OnDisable()
    {
        growing = false;
    }
    
    private void OnDestroy()
    {
        // Clean up any references
        parentCrystal = null;
    }
}