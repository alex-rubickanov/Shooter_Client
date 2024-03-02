using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject playerPawn;
    
    private PlayerMovement playerMovement;
    private PlayerAiming playerAiming;
    private PlayerShooting playerShooting;
    private PlayerAnimatorController playerAnimatorController;

}
