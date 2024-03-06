using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private string socketString;
    public string SocketString => socketString;
    public void SetSocketString(string socketString)
    {
        this.socketString = socketString;
    }
}
