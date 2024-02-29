using System;
using System.Diagnostics;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimatorController : PlayerComponent
{
    private Animator animator;
    private static readonly int VelocityMagnitude = Animator.StringToHash("VelocityMagnitude");
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    private Vector3 movementVelocity;
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    private static readonly int VelocityY = Animator.StringToHash("VelocityY");
    private static readonly int IsPistol = Animator.StringToHash("IsPistol");
    private static readonly int IsFiring = Animator.StringToHash("IsFiring");

    protected override void Awake()
    {
        base.Awake();
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
        bool isFiring = playerManager.PlayerShooting.IsFiring;
        animator.SetBool(IsAiming, isAiming);
        animator.SetBool(IsFiring, isFiring);
    }

    private void AimMoveAnimating()
    {
        movementVelocity = transform.InverseTransformDirection(movementVelocity);
        
        animator.SetFloat(VelocityX, movementVelocity.x);
        animator.SetFloat(VelocityY, movementVelocity.z);
    }

    public void SetAnimatorController(WeaponAnimationType weaponAnimationType)
    {
        switch (weaponAnimationType)
        {
            case WeaponAnimationType.Pistol:
                animator.SetBool(IsPistol, true);
                break;
            case WeaponAnimationType.Rifle:
                animator.SetBool(IsPistol, false);
                break;
        }
    }
}