using System;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private bool isAiming;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inputReader.OnAimEvent += Aim;
    }

    private void Update()
    {
        if (!isAiming)
        {
            LookWhereMove();
        }
    }

    private void Aim(bool isAiming, ControlScheme controlScheme)
    {
        this.isAiming = isAiming;
        if (controlScheme == ControlScheme.Gamepad)
        {
            // Handle Gamepad aiming
        }
        else if (controlScheme == ControlScheme.Keyboard)
        {
            // Handle Keyboard aiming
        }
    }

    private void LookWhereMove()
    {
        Vector3 moveDir = playerMovement.GetMoveDirection();
        if (moveDir != Vector3.zero)
        {
            transform.forward = moveDir;
        }
    }
}