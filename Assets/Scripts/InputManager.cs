using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    private Vector2 movementInput;
    private bool jumpInput;
    private bool isGrounded = true;
    private bool isRunning = false;
    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runningSpeedMultiplier = 1.8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float fallingGravity = 5f;
    [SerializeField] private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // PhysicMaterial noFriction = new PhysicMaterial();
        // noFriction.dynamicFriction = 0f;
        // noFriction.staticFriction = 0f;
        // noFriction.frictionCombine = PhysicMaterialCombine.Minimum;

        // Collider col = GetComponent<Collider>();
        // col.material = noFriction;
    }

    void Update() 
    {
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpInput = false;
        }
        else if(!isGrounded) // If not grounded, apply additional gravity
        {
            rb.AddForce(Vector3.down * fallingGravity, ForceMode.Acceleration);
        }
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
        moveDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection;
        float speed = isRunning ? moveSpeed * runningSpeedMultiplier : moveSpeed;
        rb.MovePosition(transform.position + moveDirection * Time.deltaTime * speed);
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Movement.movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Movement.jump.performed += i => jumpInput = true;
            playerControls.Movement.run.performed += _ => isRunning = true;
            playerControls.Movement.run.canceled += _ => isRunning = false;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }   

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private void onApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
