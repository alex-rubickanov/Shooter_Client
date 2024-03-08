using UnityEngine;

public class CloneAnimatorController : PlayerAnimatorController
{
    [SerializeField] private CloneMovement cloneMovement;

    public override void Update()
    {
        movementVelocity = cloneMovement.GetMovementVelocity();
        
        SimpleMoveAnimating();
    }
}
