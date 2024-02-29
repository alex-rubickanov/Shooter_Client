using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    private PlayerMovement playerMovement;
    private PlayerAiming playerAiming;
    private PlayerShooting playerShooting;
    private PlayerAnimatorController playerAnimatorController;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerAiming PlayerAiming => playerAiming;
    public PlayerShooting PlayerShooting => playerShooting;
    public PlayerAnimatorController PlayerAnimatorController => playerAnimatorController;
    
    
    private void Awake()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerAiming = GetComponentInChildren<PlayerAiming>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        playerAnimatorController = GetComponentInChildren<PlayerAnimatorController>();
    }

    public InputReader GetPlayerInputReader()
    {
        return inputReader;
    }
}
