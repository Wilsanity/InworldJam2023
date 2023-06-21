using UnityEditor;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator animator;
    public InputHandler inputHandler;
    MovementController playerMovement;
    int vertical;
    int horizontal;
    public bool canRotate = true;


    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();   
        playerMovement = GetComponent<MovementController>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorvalues(float verticalMovement, float horizontalMovement)
    {
        if (verticalMovement > 1)
            verticalMovement = 1;

        if (horizontalMovement > 1)
            horizontalMovement = 1;

        animator.SetFloat(vertical, Mathf.Clamp(verticalMovement,0, 1), 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, Mathf.Clamp(horizontalMovement, 0, 1), 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimatioon(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void CanRotate()
    {
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
    }

    private void OnAnimatorMove()
    {
        if (inputHandler.isInteracting == false)
            return;

        /*float delta = Time.deltaTime;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerMovement.controller.attachedRigidbody.velocity = velocity;*/
    }

}
