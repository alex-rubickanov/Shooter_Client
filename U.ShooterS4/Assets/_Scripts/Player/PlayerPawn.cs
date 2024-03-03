using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    [SerializeField] private GameInput.IGameplayActions inputReader;
    private PlayerManager playerManager;
    
    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public void DestroyPawn()
    {
        playerManager.RespawnPlayerPawn();
    }
    
    public GameInput.IGameplayActions GetInputReader()
    {
        return inputReader;
    }
}
