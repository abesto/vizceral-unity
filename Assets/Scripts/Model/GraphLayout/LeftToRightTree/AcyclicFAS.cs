using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.GraphLayout.LeftToRightTree
{
    class AcyclicFAS<E, N> where N: Node where E: Edge<N>
    {
        private HashSet<N> Stack;
        private HashSet<N> Visited;
        private Graph<E, N> Graph;
        private HashSet<E> Reversed;

        public void RemoveAcyclicEdges(Graph<E, N> graph)
        {
            Stack = new HashSet<N>();
            Visited = new HashSet<N>();
            Reversed = new HashSet<E>();
            this.Graph = graph;
            foreach (N node in graph.Nodes)
            {
                DFS(node);
            }
        }

        private void DFS(N node)
        {
            if (Visited.Contains(node))
            {
                return;
            }
            Visited.Add(node);
            Stack.Add(node);
            foreach (E edge in Graph.OutEdges(node))
            {
                if (Stack.Contains(edge.Target))
                {
                    Reverse(edge);
                }
                else
                {
                    DFS(edge.Target);
                }
            }
            Stack.Remove(node);
        }

        private void Reverse(E edge)
        {
            var oldSource = edge.Source;
            edge.Source = edge.Target;
            edge.Target = oldSource;
            Reversed.Add(edge);
        }

        public void Restore()
        {
            foreach (E edge in Reversed)
            {
                Reverse(edge);
            }
        }
    }
}
