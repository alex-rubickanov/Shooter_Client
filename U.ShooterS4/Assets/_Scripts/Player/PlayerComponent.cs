using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected InputReader inputReader;

    private void Awake()
    {
        PlayerManager playerManager = GetComponentInParent<PlayerManager>();
        inputReader = playerManager.GetPlayerInputReader();
    }
}
