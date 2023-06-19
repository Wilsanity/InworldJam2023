using UnityEditor;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator animator;
    int vertical;
    int horizontal;


    private void Start()
    {
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

}
