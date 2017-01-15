using System;

namespace Model
{

    [Serializable]
    public class Global
    {
        public string renderer;
        public string name;
        public long serverUpdateTime;
        public Region[] nodes;
        public Connection[] connections;
    }
}