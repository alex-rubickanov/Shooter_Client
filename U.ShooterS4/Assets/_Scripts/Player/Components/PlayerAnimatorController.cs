using UnityEngine;

public class PlayerAnimatorController : PlayerComponent
{
    private Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float speed = playerManager.PlayerMovement.GetCurrentSpeed();
        
        animator.SetFloat(Speed, speed);
        animator.SetBool(IsAiming, playerManager.PlayerAiming.IsAiming);
    }
}
