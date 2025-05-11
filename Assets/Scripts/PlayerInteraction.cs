using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Crystal currentCrystal;
    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        // controls.Interactions.RotateY.performed += _ => RotateY();
        controls.Interactions.RotateZ.performed += _ => RotateZ();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    // void RotateY()
    // {
    //     currentCrystal?.RotateY();
    // }

    void RotateZ()
    {
        currentCrystal?.RotateZ();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            currentCrystal = other.GetComponent<Crystal>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            if (other.GetComponent<Crystal>() == currentCrystal)
                currentCrystal = null;
        }
    }
}
