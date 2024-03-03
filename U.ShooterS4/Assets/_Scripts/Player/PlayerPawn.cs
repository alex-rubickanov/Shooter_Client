using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    private PlayerManager playerManager;
    
    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public void DestroyPawn()
    {
        playerManager.RespawnPlayerPawn();
    }
}
