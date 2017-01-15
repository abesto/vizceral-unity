using System.Collections.Generic;
using System.Linq;

namespace Model.GraphLayout
{
    public abstract class Graph<E, N> where N: Node where E: Edge<N>
    {
        public abstract N[] Nodes { get; set; }
        public abstract E[] Edges { get; set; }

        public N[] EntryNodes()
        {
            return (from node in Nodes
                   where InEdges(node).Length == 0
                   select node).ToArray();
        }

        public E[] InEdges(N target)
        {
            return (from edge in Edges
                   where edge.Target.Equals(target)
                   select edge).ToArray();
                   
        }
        public E[] OutEdges(N source)
        {
            return (from edge in Edges
                   where edge.Source.Equals(source)
                   select edge).ToArray();
        }
    }
}
