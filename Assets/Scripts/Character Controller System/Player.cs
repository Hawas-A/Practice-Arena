using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private CharacterController playerController;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private PlayerInput playerInput;
  
    public static Player Instance {  get; private set; }
    
    [Header("Movement Settings")]
    [SerializeField] private float sprintMultiplier = 2.0f;
    private float currentSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 velocity;
    private Transform cameraTransform;
   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple Player instances detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];

        playerController = GetComponentInChildren<CharacterController>();

        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
      
        Move();
        Jump();
        HandleRotation();
    }

    public void Sprint()
    {
        bool isSprinting = sprintAction.IsPressed();   
        currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
    }
  

    public void Move()
    {

        Vector2 dir2D = moveAction.ReadValue<Vector2>();
        Sprint();
        Vector3 camForward, camRight;
        SetupCameraDirections(out camForward, out camRight);
        Vector3 moveDir = camForward * dir2D.y + camRight * dir2D.x;
        moveDir.Normalize();


        Vector3 move = moveDir * currentSpeed + Vector3.up * velocity.y;


        playerController.Move(move * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                  Quaternion.LookRotation(moveDir),
                                                  10f * Time.deltaTime);
        }
    }

    public void Jump()
    {
    
        if (jumpAction.triggered && playerController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
        velocity.y += gravity * Time.deltaTime;

    }

    private void HandleRotation()
    {
        
        Vector3 camForward, camRight;
        SetupCameraDirections(out camForward, out camRight);

        Vector3 lookDirection = camForward + camRight ;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }
    }

    private void SetupCameraDirections(out Vector3 camForward, out Vector3 camRight)
    {
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;
        camForward.y = 0f; camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
    }
}
