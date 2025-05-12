using UnityEngine;

public class Crystal : MonoBehaviour
{
    public enum CrystalState { Inactive, Active }
    private CrystalState state = CrystalState.Inactive;

    [Header("References")]
    [SerializeField] private Transform model;
    [SerializeField] private Renderer crystalRenderer;
    [SerializeField] private Transform beaconOrigin;
    [SerializeField] private GameObject beaconPrefab;

    [Header("Visuals")]
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Color activeColor = Color.white;

    [Header("Rotation")]
    [SerializeField] private float zStepAngle = 22.5f;

    private GameObject currentBeacon;
    private Beacon incomingBeacon;
    private int currentZStep;
    private int maxZSteps;

    private void Start()
    {
        maxZSteps = Mathf.RoundToInt(360f / zStepAngle);
        UpdateVisuals();
        UpdateBeaconOriginRotation();
    }

    public void RotateZ()
    {
        currentZStep = (currentZStep + 1) % maxZSteps;
        model.Rotate(0f, 0f, zStepAngle);
        UpdateBeaconOriginRotation();
        UpdateBeaconDirection();
    }

    public void Activate(Beacon source)
    {
        if (state == CrystalState.Active) return;

        incomingBeacon = source;
        state = CrystalState.Active;
        UpdateVisuals();

        EmitBeacon();
    }

    public void Deactivate(Beacon source)
    {
        if (incomingBeacon != source) return;

        incomingBeacon = null;
        state = CrystalState.Inactive;
        UpdateVisuals();

        if (currentBeacon != null)
            Destroy(currentBeacon);
    }

    private void EmitBeacon()
    {
        if (!beaconPrefab || !beaconOrigin) return;

        currentBeacon = Instantiate(beaconPrefab, beaconOrigin.position, beaconOrigin.rotation);
        Beacon beacon = currentBeacon.GetComponent<Beacon>();

        if (beacon && TryGetComponent(out Collider selfCollider) && currentBeacon.TryGetComponent(out Collider beaconCollider))
        {
            Physics.IgnoreCollision(beaconCollider, selfCollider);
        }
    }

    private void UpdateBeaconDirection()
    {
        if (currentBeacon == null) return;

        Beacon beacon = currentBeacon.GetComponent<Beacon>();
        if (beacon != null)
        {
            beacon.ResetGrowth(beaconOrigin.position, beaconOrigin.rotation);
        }
    }

    private void UpdateBeaconOriginRotation()
    {
        Quaternion zRotation = Quaternion.Euler(0f, 0f, currentZStep * zStepAngle);
        beaconOrigin.localRotation = zRotation;
    }

    private void UpdateVisuals()
    {
        if (crystalRenderer != null)
        {
            crystalRenderer.material.color = (state == CrystalState.Active) ? activeColor : inactiveColor;
        }
    }
}
