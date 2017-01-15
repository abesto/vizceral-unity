using System;

namespace Model.GraphLayout
{
    public abstract class Edge<N> where N: Node
    {
        public N Source;
        public N Target;

        public string ID
        {
            get
            {
                return Source.ID + " -> " + Target.ID;
            }
        }

        public bool IsSameEdge()
        {
            return Source.Equals(Target);
        }
    }
}
