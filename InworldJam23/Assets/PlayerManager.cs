using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    InputHandler inputHandler;
    Animator animator;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponent<Animator>();
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

}