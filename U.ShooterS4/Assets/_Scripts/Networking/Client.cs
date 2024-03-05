using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    private Socket clientSocket;

    private void Start()
    {
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting to server...");
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect("127.00.1", 3000);
    }

    private void Update()
    {
        if (!clientSocket.Connected) return;
        
    }
}