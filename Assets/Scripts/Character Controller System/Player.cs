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

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f; camRight.y = 0f;
        camForward.Normalize(); 
        camRight.Normalize();
        Vector3 moveDir = camForward * dir2D.y + camRight * dir2D.x;
        moveDir.Normalize(); 

        
        Vector3 move = moveDir * currentSpeed + Vector3.up * velocity.y;

       
        playerController.Move(move * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
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

        if (playerController.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    private void HandleRotation()
    {
        Vector2 dir2D = moveAction.ReadValue<Vector2>();

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 lookDirection = camForward * dir2D.y + camRight * dir2D.x;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }
        else
        {
           
            Vector3 cameraYaw = new Vector3(camForward.x, 0, camForward.z);
            if (cameraYaw.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(cameraYaw);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
            }
        }
    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void ApplyDamage()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage()
    {
        throw new System.NotImplementedException();
    }
}
