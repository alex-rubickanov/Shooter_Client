using System;
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
        //SpawnPlayerPawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentPlayerPawn == null)
        {
            SpawnPlayerPawn();
        }
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

        ShooterNetwork.Vector2 pos = new ShooterNetwork.Vector2(currentPlayerPawn.transform.position.x,
            currentPlayerPawn.transform.position.z);
        PawnSpawnPacket psp = new PawnSpawnPacket(pos, Client.Instance.PlayerData);
        Client.Instance.SendPacket(psp);
    }
}