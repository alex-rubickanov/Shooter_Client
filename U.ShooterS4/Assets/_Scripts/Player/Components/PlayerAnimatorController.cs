using UnityEngine;

public class PlayerAnimatorController : PlayerComponent
{
    private Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float speed = playerManager.PlayerMovement.GetCurrentSpeed();
        
        animator.SetFloat(Speed, speed);

    }
}
