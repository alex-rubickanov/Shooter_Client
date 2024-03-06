using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected PlayerManager playerManager;
    protected InputReader inputReader;

    protected virtual void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        inputReader = playerManager.GetPlayerInputReader();
    }
}