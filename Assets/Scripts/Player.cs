using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private CharacterController playerController;
    private InputAction moveAction;
    private PlayerInput playerInput;


    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        playerController = GetComponentInChildren<CharacterController>();

    }
    private void Update()
    {
        Move();
    }


    private void Move()
    {
        Vector2 direction =  moveAction.ReadValue<Vector2>();
        Vector3 direction3D = new Vector3(direction.x, 0 , direction.y);
        playerController.Move(direction3D* moveSpeed * Time.deltaTime);
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
