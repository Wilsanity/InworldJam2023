using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponent<AnimatorHandler>();
    }

    public void HandleLightAttack(ItemObject weapon)
    {
        animatorHandler.PlayTargetAnimatioon(weapon.attackAnimationString, true);
    }
}
