namespace ShooterServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server(3000);
            server.Start();
        }
    }
}