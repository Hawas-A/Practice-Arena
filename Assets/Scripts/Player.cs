using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private CharacterController playerController;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private PlayerInput playerInput;

    [Header("Movement Settings")]
    [SerializeField] private float sprintMultiplier = 2.0f;
    private float currentSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 velocity; // combines horizontal + vertical

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];

        playerController = GetComponentInChildren<CharacterController>();
    }

    private void Update()
    {
      
        Move();
        Jump();
    }

   
    public void Move()
    {
        Vector2 dir2D = moveAction.ReadValue<Vector2>();
        Sprint();
        Vector3 horizontalMove = new Vector3(dir2D.x, 0, dir2D.y) * currentSpeed;
        Vector3 move = horizontalMove + new Vector3(0, velocity.y, 0);
        playerController.Move(move * Time.deltaTime);
    }

    public void Sprint()
    {
        bool isSprinting = sprintAction.IsPressed();   
        currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
    }

    public void Jump()
    {
        if (jumpAction.triggered && playerController.isGrounded)
        {

            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }


        velocity.y += gravity * Time.deltaTime;

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
