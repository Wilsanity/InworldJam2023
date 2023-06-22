using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    InputHandler inputHandler;
    MovementController controller;
    public Health health;
    public Animator animator;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        controller = GetComponent<MovementController>();
    }

    private void Start()
    {
        health.OnDeath += HandlePlayerDeath;
    }

    private void Update()
    {
        inputHandler.HandleAllInputs();
        inputHandler.isInteracting = animator.GetBool("isInteracting");
    }

    private void FixedUpdate()
    {
        //playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        inputHandler.attackInput = false;

        //cameraManager.HandleAllCameraMovement();
    }

    private void HandlePlayerDeath()
    {
        inputHandler.enabled = false;
        controller.enabled = false;
    }
}