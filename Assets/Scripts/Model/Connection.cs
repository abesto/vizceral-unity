using System;

namespace Model
{
    [Serializable]
    public class Connection
    {
        public string source;
        public string target;
        public Metrics metrics;
        public string @class;
    }
}

