using System.Collections;
using ShooterNetwork;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private PlayerPawn playerPawnPrefab;
    [SerializeField] private float respawnTime;

    [SerializeField] private Transform[] spawnPoints;

    private PlayerMovement playerMovement;
    private PlayerAiming playerAiming;
    private PlayerShooting playerShooting;
    private PlayerAnimatorController playerAnimatorController;
    private PlayerPawn currentPlayerPawn;

    private void Start()
    {
        if (Client.Instance.disableServerConnection)
        {
            SimpleSpawn();
        }

        Client.Instance.OnIDAssigned += OnIDAssigned;
        Client.Instance.OnStartGamePacketReceived += OnStartGamePacketReceived;
    }

    private void OnStartGamePacketReceived(StartGamePacket obj)
    {
        SpawnPlayerPawn();
    }

    private void OnIDAssigned()
    {
        //SpawnPlayerPawn();
    }

    public void RespawnPlayerPawn()
    {
        StartCoroutine(RespawnWithDelay());
    }

    private IEnumerator RespawnWithDelay()
    {
        Destroy(currentPlayerPawn.gameObject);
        yield return new WaitForSeconds(respawnTime);
        //currentPlayerPawn = null;
        SpawnPlayerPawn();
    }

    public void SpawnPlayerPawn()
    {
        if (currentPlayerPawn != null)
        {
            Destroy(currentPlayerPawn.gameObject);
        }

        int spawnPointIndex = int.Parse(Client.Instance.PlayerData.ID);
        currentPlayerPawn = Instantiate(playerPawnPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity,
            transform);
        currentPlayerPawn.SetPlayerManager(this);
        currentPlayerPawn.GetInputReader().EnableGameplayInput();

        ShooterNetwork.Vector2 pos = new ShooterNetwork.Vector2(currentPlayerPawn.transform.position.x,
            currentPlayerPawn.transform.position.z);

        PawnSpawnPacket psp = new PawnSpawnPacket(pos, Client.Instance.PlayerData);
        Client.Instance.SendPacket(psp);
    }

    public void SimpleSpawn()
    {
        currentPlayerPawn = Instantiate(playerPawnPrefab, spawnPoints[0].position, Quaternion.identity, transform);
        currentPlayerPawn.SetPlayerManager(this);
        currentPlayerPawn.GetInputReader().EnableGameplayInput();
    }
}