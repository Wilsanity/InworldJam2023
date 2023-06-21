using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public MovementController movementController;
    //public CameraController cameraController;

    //When a character is in an animation, disable other forms of movement
    public bool isInteracting;

    public bool attackInput = false;

    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    public PlayerAttacker playerAttacker;
    public ItemObject weapon;

    public void Start()
    {
        enabled = true;

        Controls.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        Controls.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        Controls.Player.Attack.performed += ctx => attackInput = true;
    }

    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();

    public Vector2 movementInput;

    public float verticalInput;
    public float horizontalInput;

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleAttackInput();
        //HandleJumpingInput
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }

    private void HandleAttackInput()
    {
        if (attackInput)
        {
            playerAttacker.HandleLightAttack(weapon);
        }   
    }

}
