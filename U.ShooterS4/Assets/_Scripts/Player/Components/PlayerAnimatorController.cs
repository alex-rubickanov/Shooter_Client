using UnityEngine;

public class PlayerAnimatorController : PlayerComponent
{
    private Animator animator;
    private static readonly int VelocityMagnitude = Animator.StringToHash("VelocityMagnitude");
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    private Vector3 movementVelocity;
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    private static readonly int VelocityY = Animator.StringToHash("VelocityY");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movementVelocity = playerManager.PlayerMovement.GetMovementVelocity();
        
        SimpleMoveAnimating();
        AimAnimating();
        AimMoveAnimating();
    }

    private void SimpleMoveAnimating()
    {
        float maxSpeed = playerManager.PlayerMovement.MaxSpeed;
        animator.SetFloat(VelocityMagnitude, movementVelocity.magnitude / maxSpeed);
    }
    
    private void AimAnimating()
    {
        bool isAiming = playerManager.PlayerAiming.IsAiming;
        animator.SetBool(IsAiming, isAiming);
    }

    private void AimMoveAnimating()
    {
        movementVelocity = transform.InverseTransformDirection(movementVelocity);
        
        animator.SetFloat(VelocityX, movementVelocity.x);
        animator.SetFloat(VelocityY, movementVelocity.z);
    }
}