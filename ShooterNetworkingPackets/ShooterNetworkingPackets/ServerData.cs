namespace ShooterNetwork
{
    public struct ServerData : IDataHolder
    {
        public ServerData(string name = "Server", string id = "0")
        {
            this.name = name;
            this.id = id;
        }

        private string name;
        private string id;
        
        public string Name => name;
        public string ID => id.ToString();
    }
}