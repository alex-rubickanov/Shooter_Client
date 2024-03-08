using UnityEngine;
using System;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] protected RuntimeAnimatorController pistolAnimatorController;
    [SerializeField] protected RuntimeAnimatorController rifleAnimatorController;
    
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAiming playerAiming;
    [SerializeField] private PlayerShooting playerShooting;
    
    protected Animator animator;
    protected static readonly int VelocityMagnitude = Animator.StringToHash("VelocityMagnitude");
    protected static readonly int IsAiming = Animator.StringToHash("IsAiming");

    protected Vector3 movementVelocity;
    protected static readonly int VelocityX = Animator.StringToHash("VelocityX");
    protected static readonly int VelocityY = Animator.StringToHash("VelocityY");
    protected static readonly int IsPistol = Animator.StringToHash("IsPistol");
    protected static readonly int IsFiring = Animator.StringToHash("IsFiring");
    protected static readonly int Reload = Animator.StringToHash("Reload");
    protected static readonly int ReloadTimeMultiplayer = Animator.StringToHash("ReloadTimeMultiplier");

    public event Action OnReload1;
    public event Action OnReload2;
    public event Action OnReload3;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        movementVelocity = playerMovement.GetMovementVelocity();

        SimpleMoveAnimating();
        AimAnimating();
        AimMoveAnimating();
    }

    protected void SimpleMoveAnimating()
    {
        float maxSpeed = PlayerConstants.MAX_SPEED;
        animator.SetFloat(VelocityMagnitude, movementVelocity.magnitude / maxSpeed);
    }

    protected virtual void AimAnimating()
    {
        bool isAiming = playerAiming.IsAiming;
        bool isFiring = playerShooting.IsFiring;
        animator.SetBool(IsAiming, isAiming);
        animator.SetBool(IsFiring, isFiring);
    }

    protected void AimMoveAnimating()
    {
        movementVelocity = transform.InverseTransformDirection(movementVelocity);

        animator.SetFloat(VelocityX, movementVelocity.x);
        animator.SetFloat(VelocityY, movementVelocity.z);
    }

    public void PlayReloadAnimation(float duration)
    {
        if (duration == 0) return;
        animator.SetLayerWeight(1, 100);
        animator.SetFloat(ReloadTimeMultiplayer, 3 / duration);
        animator.SetTrigger(Reload);
    }

    public void SetAnimatorController(WeaponAnimationType weaponAnimationType)
    {
        switch (weaponAnimationType)
        {
            case WeaponAnimationType.Pistol:
                animator.runtimeAnimatorController = pistolAnimatorController;
                break;
            case WeaponAnimationType.Rifle:
                animator.runtimeAnimatorController = rifleAnimatorController;
                break;
        }
    }

    public void OnReloadAnimationEnd()
    {
        animator.SetLayerWeight(1, 0);
    }

    public void Reload1()
    {
        OnReload1?.Invoke();
    }

    public void Reload2()
    {
        OnReload2?.Invoke();
    }

    public void Reload3()
    {
        OnReload3?.Invoke();
    }
}