using System.Net.Sockets;

public class ConnectionPacket : BasePacket
{
    private string clientSocketString;
    public string ClientSocketString => clientSocketString;
    
    public ConnectionPacket()
    {
        clientSocketString = "";
    }
    
    public ConnectionPacket(Socket clientSocket) : base(PacketType.ConnectionPacket)
    {
        this.clientSocketString = clientSocket.ToString();
    }
    
    public override byte[] Serialize()
    {
        BeginSerialize();
        sbw.Write(clientSocketString);
        return EndSerialize();
    }
    
    public new ConnectionPacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        clientSocketString = dbr.Read().ToString();
        EndDeserialize();
        return this;
    }
}