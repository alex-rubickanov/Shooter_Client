using System.Collections;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField]private InputReader inputReader;
    [SerializeField] private float timeBeforeDestroy = 3.0f;
    
    
    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        DestroyPawn();
    }

    public void DestroyPawn()
    {
        playerManager.RespawnPlayerPawn();
    }
    
    public InputReader GetInputReader()
    {
        return inputReader;
    }
}
