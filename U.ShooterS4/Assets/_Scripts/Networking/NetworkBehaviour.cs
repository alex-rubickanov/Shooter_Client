using UnityEngine;
using ShooterNetwork;
using Vector2 = ShooterNetwork.Vector2;

public class NetworkBehaviour : MonoBehaviour
{
    public void SendDebugLogPacket(string message)
    {
        DebugLogPacket debugLogPacket = new DebugLogPacket(message, Client.Instance.PlayerData);
        Client.Instance.SendPacket(debugLogPacket);
    }

    public void SendMovePacket(UnityEngine.Vector3 position)
    {
        Vector2 vec = new Vector2(position.x, position.z);
        MovePacket movePacket = new MovePacket(vec, Client.Instance.PlayerData);
        Client.Instance.SendPacket(movePacket);
    }
}