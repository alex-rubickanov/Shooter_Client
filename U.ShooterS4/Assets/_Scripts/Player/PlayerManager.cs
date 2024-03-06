using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    private PlayerMovement playerMovement;
    private PlayerAiming playerAiming;
    private PlayerShooting playerShooting;

    public PlayerMovement PlayerMovement => playerMovement;
    public PlayerAiming PlayerAiming => playerAiming;
    public PlayerShooting PlayerShooting => playerShooting;
    
    
    private void Start()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerAiming = GetComponentInChildren<PlayerAiming>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
    }

    public InputReader GetPlayerInputReader()
    {
        return inputReader;
    }
}
