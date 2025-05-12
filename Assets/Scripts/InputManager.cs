using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    private Vector2 movementInput;
    private bool jumpInput;
    private bool isGrounded = true;
    private bool isRunning = false;
    private Rigidbody rb;
    private Animator animator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runningSpeedMultiplier = 1.8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float fallingGravity = 5f;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float walkThreshold = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleJump();
        HandleMovement();
        UpdateAnimator();
    }

    void HandleJump()
    {
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("JumpTrigger");
            isGrounded = false;
            jumpInput = false;
        }

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * fallingGravity, ForceMode.Acceleration);
        }
    }

    void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
        moveDirection = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * moveDirection;

        float speed = isRunning && isGrounded ? moveSpeed * runningSpeedMultiplier : moveSpeed;
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        if (movementInput.sqrMagnitude > 0.01f)
        {
            Vector3 lookDirection = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }

    void UpdateAnimator()
    {
        float horizontalSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Speed", horizontalSpeed);
        animator.SetBool("IsRunning", isRunning && horizontalSpeed > walkThreshold);
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Movement.movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Movement.movement.canceled += _ => movementInput = Vector2.zero;
            playerControls.Movement.jump.performed += _ => jumpInput = true;
            playerControls.Movement.run.performed += _ => isRunning = true;
            playerControls.Movement.run.canceled += _ => isRunning = false;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
        {
            isGrounded = false;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

// --------- claude - jump not work, rest good --------------
// using UnityEngine;

// public class InputManager : MonoBehaviour
// {
//     PlayerControls playerControls;
//     private Vector2 movementInput;
//     private bool jumpInput;
//     private bool isGrounded = true;
//     private bool isRunning = false;
//     private Rigidbody rb;
//     [SerializeField] private float moveSpeed = 5f;
//     [SerializeField] private float runningSpeedMultiplier = 1.8f;
//     [SerializeField] private float jumpForce = 5f;
//     [SerializeField] private float fallingGravity = 5f;
//     [SerializeField] private Transform cameraTransform;
//     private Animator animator;
    
//     // Threshold values for animations
//     [SerializeField] private float walkThreshold = 0.1f;
//     [SerializeField] private float runThreshold = 3.0f; // Increased from 0.3

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         animator = GetComponent<Animator>();
//         rb.freezeRotation = true;
//     }

//     void Update() 
//     {
//         HandleJump();
//         HandleMovement();
//         UpdateAnimator();
//     }

//     void HandleJump()
//     {
//         if (jumpInput && isGrounded)
//         {
//             rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//             animator.SetBool("IsJumping", true);
//             isGrounded = false;
//             jumpInput = false;
//         }
//         else if(!isGrounded) // If not grounded, apply additional gravity
//         {
//             rb.AddForce(Vector3.down * fallingGravity, ForceMode.Acceleration);
//         }
//     }

//     void HandleMovement()
//     {
//         Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
//         // Fixed typo: caameraTransform -> cameraTransform
//         moveDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection;

//         float speed = isRunning && isGrounded ? moveSpeed * runningSpeedMultiplier : moveSpeed;
        
//         rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
//     }

//     void UpdateAnimator()
//     {
//         // Calculate movement speed (excluding vertical movement)
//         float horizontalSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        
//         // Debug movement values
//         Debug.Log($"Speed: {horizontalSpeed}, IsRunning: {isRunning}, IsGrounded: {isGrounded}, IsJumping: {animator.GetBool("IsJumping")}");
        
//         // Set the Speed parameter more directly for better animation control
//         animator.SetFloat("Speed", horizontalSpeed);
        
//         // Explicitly set running state for clearer animation control
//         animator.SetBool("IsRunning", isRunning && horizontalSpeed > walkThreshold);
//     }

//     private void OnEnable()
//     {
//         if (playerControls == null)
//         {
//             playerControls = new PlayerControls();
//             playerControls.Movement.movement.performed += i => movementInput = i.ReadValue<Vector2>();
//             playerControls.Movement.movement.canceled += i => movementInput = Vector2.zero; // Reset when input stops
//             playerControls.Movement.jump.performed += i => jumpInput = true;
//             playerControls.Movement.run.performed += _ => isRunning = true;
//             playerControls.Movement.run.canceled += _ => isRunning = false;
//         }
//         playerControls.Enable();
//     }

//     private void OnDisable()
//     {
//         playerControls.Disable();
//     }   

//     void OnCollisionEnter(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
//         {
//             animator.SetBool("IsJumping", false);
//             isGrounded = true;
//         }
//     }

//     void OnCollisionStay(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
//         {
//             isGrounded = true;
//         }
//     }

//     void OnCollisionExit(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
//         {
//             isGrounded = false;
//         }
//     }
    
//     // Fixed method name casing (should be OnApplicationFocus)
//     private void OnApplicationFocus(bool focus)
//     {
//         if (focus)
//         {
//             Cursor.lockState = CursorLockMode.Locked;
//         }
//         else
//         {
//             Cursor.lockState = CursorLockMode.None;
//         }
//     }
// }

// --------- old --------------
    // using UnityEngine;

    // public class InputManager : MonoBehaviour
    // {
    //     PlayerControls playerControls;
    //     private Vector2 movementInput;
    //     private bool jumpInput;
    //     private bool isGrounded = true;
    //     private bool isRunning = false;
    //     private Rigidbody rb;
    //     [SerializeField] private float moveSpeed = 5f;
    //     [SerializeField] private float runningSpeedMultiplier = 1.8f;
    //     [SerializeField] private float jumpForce = 5f;
    //     [SerializeField] private float fallingGravity = 5f;
    //     [SerializeField] private Transform cameraTransform;
    //     private Animator animator;

    //     void Start()
    //     {
    //         rb = GetComponent<Rigidbody>();
    //         animator = GetComponent<Animator>();
    //         rb.freezeRotation = true;
    //     }

    //     void Update() 
    //     {
    //         if (jumpInput && isGrounded)
    //         {
    //             rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //             animator.SetBool("IsJumping", true);
    //             jumpInput = false;
    //         }
    //         else if(!isGrounded) // If not grounded, apply additional gravity
    //         {
    //             rb.AddForce(Vector3.down * fallingGravity, ForceMode.Acceleration);
    //         }

    //         Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
    //         moveDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection;

    //         float speed = isRunning && isGrounded ? moveSpeed * runningSpeedMultiplier : moveSpeed;
            
    //         // rb.MovePosition(transform.position + moveDirection * Time.deltaTime * speed);
    //         rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

    //         // Animate
    //         animator.SetFloat("Speed", new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude);

    //     }

    //     private void OnEnable()
    //     {
    //         if (playerControls == null)
    //         {
    //             playerControls = new PlayerControls();
    //             playerControls.Movement.movement.performed += i => movementInput = i.ReadValue<Vector2>();
    //             playerControls.Movement.jump.performed += i => jumpInput = true;
    //             playerControls.Movement.run.performed += _ => isRunning = true;
    //             playerControls.Movement.run.canceled += _ => isRunning = false;
    //         }
    //         playerControls.Enable();
    //     }

    //     private void OnDisable()
    //     {
    //         playerControls.Disable();
    //     }   

    //     void OnCollisionStay(Collision collision)
    //     {
    //         if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
    //         {
    //             animator.SetBool("IsJumping", false);
    //             isGrounded = true;
    //         }
    //     }

    //     void OnCollisionExit(Collision collision)
    //     {
    //         if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GhostPlatform"))
    //         {
    //             isGrounded = false;
    //         }
    //     }
    //     private void onApplicationFocus(bool focus)
    //     {
    //         if (focus)
    //         {
    //             Cursor.lockState = CursorLockMode.Locked;
    //         }
    //         else
    //         {
    //             Cursor.lockState = CursorLockMode.None;
    //         }
    //     }
    // }
