using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private AudioManagerChannel sfxChannel;
    [SerializeField] private AudioClip unknownMusic;
    [SerializeField] private AudioClip macarenaMusic;
    [SerializeField] private AudioClip waveMusic;
    [SerializeField] private AudioClip snakeMusic;
    
    [SerializeField] protected RuntimeAnimatorController pistolAnimatorController;
    [SerializeField] protected RuntimeAnimatorController rifleAnimatorController;
    [SerializeField] protected RuntimeAnimatorController danceAnimatorController;
    
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

    protected RuntimeAnimatorController cachedController;

    protected bool isDancing = false;

    public event Action<int> OnDanceStart;
    public event Action OnDanceEnd;

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
        if(isDancing) return;
        float maxSpeed = PlayerConstants.MAX_SPEED;
        animator.SetFloat(VelocityMagnitude, movementVelocity.magnitude / maxSpeed);
    }

    protected virtual void AimAnimating()
    {
        if (isDancing) return;
        bool isAiming = playerAiming.IsAiming;
        bool isFiring = playerShooting.IsFiring;
        animator.SetBool(IsAiming, isAiming);
        animator.SetBool(IsFiring, isFiring);
    }

    protected void AimMoveAnimating()
    {
        if (isDancing) return;
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
    
    public void PlayDance(int danceIndex)
    {
        if (isDancing) return;
        switch (danceIndex)
        {
            case 1:
                sfxChannel.RaiseEvent(unknownMusic, transform.position);
                break;
            case 2:
                sfxChannel.RaiseEvent(macarenaMusic, transform.position);
                break;
            case 3:
                sfxChannel.RaiseEvent(waveMusic, transform.position);
                break;
            case 4:
                sfxChannel.RaiseEvent(snakeMusic, transform.position);
                break;
            default:
                sfxChannel.RaiseEvent(waveMusic, transform.position);
                break;
        }
        OnDanceStart?.Invoke(danceIndex);
        isDancing = true;
        cachedController = animator.runtimeAnimatorController;
        animator.runtimeAnimatorController = danceAnimatorController;
        animator.SetTrigger(danceIndex.ToString());
    }

    public void OnReloadAnimationEnd()
    {
        animator.SetLayerWeight(1, 0);
    }

    public void OnDanceAnimationEnd()
    {
        isDancing = false;
        OnDanceEnd?.Invoke();
        if (cachedController != null)
        {
            animator.runtimeAnimatorController = cachedController;
        }
    }

    public void InvokeOnDanceStart(int index)
    {
        OnDanceStart?.Invoke(index);
    }
    
    public void InvokeOnDanceEnd()
    {
        OnDanceEnd?.Invoke();
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