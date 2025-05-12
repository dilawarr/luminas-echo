using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(BoxCollider))]
public class Beacon : MonoBehaviour
{
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float growSpeed = 20f;

    private LineRenderer line;
    private BoxCollider boxCollider;
    private float currentLength;
    private bool isGrowing = true;
    [SerializeField] private bool isFireBeacon = false;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        line.positionCount = 2;
        line.useWorldSpace = true;
        boxCollider.isTrigger = true;
    }

    private void Update()
    {
        if (!isGrowing) return;

        currentLength += growSpeed * Time.deltaTime;
        currentLength = Mathf.Min(currentLength, maxDistance);

        UpdateBeamVisual();
    }

    private void UpdateBeamVisual()
    {
        Vector3 start = transform.position;
        Vector3 dir = transform.right;
        Vector3 end = start + dir * currentLength;

        line.SetPosition(0, start);
        line.SetPosition(1, end);

        boxCollider.center = new Vector3(currentLength / 2f, 0f, 0f);
        boxCollider.size = new Vector3(currentLength, 0.1f, 0.1f);
    }

    public void ResetGrowth(Vector3 newPos, Quaternion newRot)
    {
        transform.SetPositionAndRotation(newPos, newRot);
        currentLength = 0f;
        isGrowing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            Crystal crystal = other.GetComponent<Crystal>() ?? other.GetComponentInParent<Crystal>();
            crystal?.Activate(this);
            isGrowing = false;
        }
        else if (other.CompareTag("TimeTrialCrystal"))
        {
            TimeTrialCrystal crystal = other.GetComponent<TimeTrialCrystal>();
            crystal?.Activate(this);
            isGrowing = false;
        }
        else if (other.CompareTag("Door"))
        {
            other.GetComponent<Door>()?.Open();
            isGrowing = false;
        }
        else if (other.CompareTag("IceWall"))
        {
            if (isFireBeacon)
            {
                IceWall ice = other.GetComponent<IceWall>();
                if (ice != null)
                {
                    ice.Melt();
                }
            }

            isGrowing = false;
        }

        else if (other.CompareTag("GhostPlatform"))
        {
            // Optional: handle ghost platform collision logic here
            isGrowing = false;
        }

        else if (other.CompareTag("Wall") || other.CompareTag("Obstacle") || other.CompareTag("Ground") || other.CompareTag("IceWall"))
        {
            isGrowing = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            Crystal crystal = other.GetComponent<Crystal>() ?? other.GetComponentInParent<Crystal>();
            crystal?.Deactivate(this);
        }
        // if (other.CompareTag("TimeTrialCrystal"))
        // {
        //     TimeTrialCrystal crystal = other.GetComponent<TimeTrialCrystal>();
        //     if (crystal != null)
        //     {
        //         crystal.Deactivate(this);
        //     }
        // }
        else if (other.CompareTag("Door"))
        {
            other.GetComponent<Door>()?.Close();
        }
    }

    private void OnDisable()
    {
        isGrowing = false;
    }
}
