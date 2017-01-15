using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.GraphLayout.LeftToRightTree
{
    public class LeftToRightTreeLayoutEngine<E, N>: LayoutEngine<E, N> where N: Node where E: Edge<N>
    {
        private float lastYDelta = 0;
        private int yOffset = -2;
        private const int maxNodesPerDepth = 30;

        private float height = 32;
        private float width = 74;

        public void SetGraph(Graph<E, N> graph)
        {
            var ranks = new Ranker<E, N>().Rank(graph);
            N[][] nodesByRank = SortNodesByRank(ranks);
            OverflowNodes(nodesByRank);
            PositionNodes(nodesByRank);
        }

        private void OverflowNodes(N[][] nodesByRank)
        {
            for (int rank = 0; rank < nodesByRank.Length; rank++)
            {
                var nodesInRank = nodesByRank[rank];
                if (nodesInRank.Length > maxNodesPerDepth)
                {
                    var nodesToKeep = nodesInRank.Take(Math.Min(nodesInRank.Length / 2 - 1, maxNodesPerDepth)).ToArray();
                    var overflowNodes = nodesInRank.Skip(nodesToKeep.Length);
                    nodesByRank[rank] = nodesToKeep;
                    nodesByRank[rank + 1] = nodesByRank[rank + 1].Concat(overflowNodes).ToArray();
                }
            }
        }

        private N[][] SortNodesByRank(Dictionary<int, LinkedList<N>> ranks)
        {
            return (from nodes in ranks.Values
                    where nodes.Any()
                    select nodes.ToArray()).ToArray();
        }

        private void PositionNodes(N[][] nodesByRank)
        {
            float xDelta;
            if (nodesByRank.Length == 1)
            {
                xDelta = width / 2;
                SetPositions(1, nodesByRank[0], xDelta);
            } else
            {
                xDelta = width / (nodesByRank.Length - 1);
                for (int i = 0; i < nodesByRank.Length; i++)
                {
                    SetPositions(i, nodesByRank[i], xDelta);
                }
            }
        }

        private void SetPositions(int column, N[] nodesAtDepth, float xDelta)
        {
            var curXDelta = xDelta * column;
            var yDelta = height / (nodesAtDepth.Length - 1);
            var needsYOffset = yDelta < lastYDelta ? lastYDelta % yDelta < 1 : yDelta % lastYDelta < 1;
            if (needsYOffset)
            {
                yOffset = -yOffset;
            }

            if (nodesAtDepth.Length == 1)
            {
                yDelta = height / 2;
            }
            for (var j = 0; j < nodesAtDepth.Length; j++)
            {
                var curYDelta = (yDelta * (j + 1)) + (needsYOffset ? yOffset : 0);
                nodesAtDepth[j].Position = new UnityEngine.Vector2(curXDelta, curYDelta);
            }

            lastYDelta = yDelta;
        }

        public void Update(float deltaTime)
        {
            // We do a layout once, then leave it alone
        }
    }
}
