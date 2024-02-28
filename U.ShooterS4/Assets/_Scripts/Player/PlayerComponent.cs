using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected InputReader inputReader;

    protected virtual void Awake()
    {
        PlayerManager playerManager = GetComponentInParent<PlayerManager>();
        inputReader = playerManager.GetPlayerInputReader();
    }
}
