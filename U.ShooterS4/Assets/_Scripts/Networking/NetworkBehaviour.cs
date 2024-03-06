using UnityEngine;

public class NetworkBehaviour : MonoBehaviour
{
    public void SendDebugLogPacket(string message)
    {
        DebugLogPacket debugLogPacket = new DebugLogPacket(message);
        Client.Instance.SendPacket(debugLogPacket);
    }
}
