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

    public void SendMovePacket(Vector2 position, Vector2 velocity, float rotationY)
    {
        MovePacket movePacket = new MovePacket(position, velocity, rotationY, Client.Instance.PlayerData);
        Client.Instance.SendPacket(movePacket);
    }
}