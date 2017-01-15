using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.GraphLayout.LeftToRightTree
{
    /* https://github.com/Netflix/vizceral/blob/master/src/layouts/ltrTreeLayout/ranker.js @ https://github.com/Netflix/vizceral/commit/e7ef9825cb1e009a7ea4cdbd16c5254b72e1f0f5 */

    class Ranker<E, N> where N: Node where E: Edge<N>
    {
        private const int minimumLength = 1;

        private Graph<E, N> graph;
        private Dictionary<N, int> ranks;
        private HashSet<N> visited;
        private E[] removedSameEdges;

        public Dictionary<int, LinkedList<N>> Rank(Graph<E, N> graph)
        {
            // Initial state
            this.graph = graph;
            ranks = new Dictionary<N, int>();
            visited = new HashSet<N>();

            // Temporarly remove same-edges and acyclic edges
            RemoveSameEdges();
            AcyclicFAS<E, N> acyclic = new AcyclicFAS<E, N>();
            acyclic.RemoveAcyclicEdges(graph);

            // Calculate ranks
            LongestPathRanking();

            // Restore same-edges and acyclic edges
            RestoreSameEdges();
            acyclic.Restore();

            // Post processing to make the graph make more sense to hoomans
            NormalizeRanks();
            ForcePrimaryRankPromotion();
            ForceSecondaryRankPromotions();

            return NodesByRank();
        }

        private void RemoveSameEdges()
        {
            removedSameEdges = graph.Edges.Where(e => e.IsSameEdge()).ToArray();
            graph.Edges = graph.Edges.Where(e => !removedSameEdges.Contains(e)).ToArray();
        }

        private void RestoreSameEdges()
        {
            graph.Edges = graph.Edges.Concat(removedSameEdges).ToArray();
        }

        private void LongestPathRanking()
        {
            foreach (N node in graph.EntryNodes())
            {
                DFS(node);
            }
        }

        private int DFS(N node)
        {
            if (!visited.Contains(node))
            {
                visited.Add(node);
                E[] outEdges = graph.OutEdges(node).ToArray();
                if (outEdges.Length == 0)
                {
                    ranks[node] = 0;
                }
                else
                {
                    ranks[node] = int.MaxValue;
                    foreach (E edge in outEdges)
                    {
                        ranks[node] = Math.Min(ranks[node], DFS(edge.Target) - minimumLength);
                    }
                }
            }
            return ranks[node];
        }

        private void NormalizeRanks()
        {
            int lowestRank = ranks.Values.Aggregate(Math.Min);
            foreach (N node in ranks.Keys.ToArray())
            {
                ranks[node] -= lowestRank;
            }
        }

        private void ForcePrimaryRankPromotion()
        {
            foreach (N node in graph.EntryNodes())
            {
                ranks[node] = 0;
            }
        }

        private void ForceSecondaryRankPromotions()
        {
            foreach (N entryNode in graph.EntryNodes())
            {
                var outNodes = from edge in graph.OutEdges(entryNode)
                               select edge.Target;
                foreach (N node in outNodes)
                {
                    ranks[node] = 1;
                }
            }
        }


        private Dictionary<int, LinkedList<N>> NodesByRank()
        {
            Dictionary<int, LinkedList<N>> retval = new Dictionary<int, LinkedList<N>>();
            foreach (var pair in ranks)
            {
                int rank = pair.Value;
                N node = pair.Key;
                if (!retval.ContainsKey(rank))
                {
                    retval.Add(rank, new LinkedList<N>());
                }
                retval[rank].AddLast(node);
            }
            return retval;
        }
    }
}
