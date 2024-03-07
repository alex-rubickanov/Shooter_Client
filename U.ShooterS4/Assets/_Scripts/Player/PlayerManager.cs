using System.Collections;
using ShooterNetwork;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private PlayerPawn playerPawnPrefab;
    [SerializeField] private float respawnTime;
    
    private PlayerMovement playerMovement;
    private PlayerAiming playerAiming;
    private PlayerShooting playerShooting;
    private PlayerAnimatorController playerAnimatorController;
    private PlayerPawn currentPlayerPawn;

    private void Start()
    {
        Client.Instance.OnIDAssigned += OnIDAssigned;
    }

    private void OnIDAssigned()
    {
        SpawnPlayerPawn();
    }


    public void RespawnPlayerPawn()
    {
        StartCoroutine(RespawnWithDelay());
    }

    private IEnumerator RespawnWithDelay()
    {
        Destroy(currentPlayerPawn.gameObject);
        currentPlayerPawn = null;
        yield return new WaitForSeconds(respawnTime);
        SpawnPlayerPawn();
    }

    private void SpawnPlayerPawn()
    {
        currentPlayerPawn = Instantiate(playerPawnPrefab, transform.position, Quaternion.identity, transform);
        currentPlayerPawn.SetPlayerManager(this);
        currentPlayerPawn.GetInputReader().EnableInput();

        PawnSpawnPacket psp = new PawnSpawnPacket(Client.Instance.PlayerData);
        Client.Instance.SendPacket(psp);
    }
}
