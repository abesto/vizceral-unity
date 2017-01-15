using Model.GraphLayout;
using System;

namespace Model
{
    [Serializable]
    public class JSONConnection
    {
        // JSON fields
        public string source;
        public string target;
        public Metrics metrics;
        public string @class;
        // EOF JSON fields
    }


    public class Connection<N>: Edge<N> where N: Node
    {
        public Metrics Metrics;
        public Metrics EmitRate;
        public string Class;

        public Connection(JSONConnection json, N source, N target)
        {
            Source = source;
            Target = target;
            Metrics = json.metrics;
        }
    }
}

