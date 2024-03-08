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

    public void SendMovePacket(float positionX, float positionY, float rotationY)
    {
        Vector2 pos = new Vector2(positionX, positionY);
        MovePacket movePacket = new MovePacket(new Vector2(positionX, positionY), rotationY, Client.Instance.PlayerData);
        Client.Instance.SendPacket(movePacket);
    }
}