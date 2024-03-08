using UnityEngine;

public class CloneAnimatorController : PlayerAnimatorController
{
    [SerializeField] private CloneMovement cloneMovement;
    [SerializeField] private CloneAiming cloneAiming;

    public override void Update()
    {
        movementVelocity = cloneMovement.GetMovementVelocity();

        SimpleMoveAnimating();
        AimAnimating();
        AimMoveAnimating();
    }

    protected override void AimAnimating()
    {
        bool isAiming = cloneAiming.IsAiming;
        animator.SetBool(IsAiming, isAiming);
    }
}