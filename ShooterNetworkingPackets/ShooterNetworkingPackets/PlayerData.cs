using System;

namespace ShooterNetwork
{
    [Serializable]
    public struct PlayerData : IDataHolder
    {
        public PlayerData(string name = "Server", string id = "0")
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