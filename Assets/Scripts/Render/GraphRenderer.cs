using EpForceDirectedGraph.cs;
using System.Collections.Generic;
using UnityEngine;

public class GraphRenderer: AbstractRenderer
{
    LoadSampleData parent;
    Dictionary<string, ServiceNode> serviceNodes;
    Dictionary<string, ServiceEdge> serviceEdges;

    public GraphRenderer(IForceDirected iForceDirected, LoadSampleData parent) : base(iForceDirected)
    {
        this.parent = parent;
        this.serviceNodes = new Dictionary<string, ServiceNode>();
        this.serviceEdges = new Dictionary<string, ServiceEdge>();
    }

    public override void Clear()
    {
        foreach (ServiceNode node in this.serviceNodes.Values)
        {
            node.gameObject.SetActive(false);
        }
        foreach (ServiceEdge edge in this.serviceEdges.Values)
        {
            // TODO doing setactive each frame resets the particle systems :( need another way to handle "gone" edges
            //edge.gameObject.SetActive(false);
        }
    }

    protected Vector2 Convert(AbstractVector v)
    {
        return new Vector2(v.x, v.y);
    }

    protected override void drawEdge(Edge iEdge, AbstractVector iPosition1, AbstractVector iPosition2)
    {
        if (!serviceEdges.ContainsKey(iEdge.ID))
        {
            serviceEdges.Add(iEdge.ID, MonoBehaviour.Instantiate<ServiceEdge>(parent.serviceEdgePrefab, Vector2.zero, Quaternion.identity));
        }
        if (serviceNodes.ContainsKey(iEdge.Source.ID) && serviceNodes.ContainsKey(iEdge.Target.ID))
        {
            serviceEdges[iEdge.ID].UpdateData(serviceNodes[iEdge.Source.ID], serviceNodes[iEdge.Target.ID], 1 / iEdge.Data.length * 0.005f, 0, 0);
        }
        //serviceEdges[iEdge.ID].gameObject.SetActive(true);
    }

    protected override void drawNode(Node iNode, AbstractVector iPosition)
    {
        Vector2 vector = new Vector2(iPosition.x, iPosition.y);
        if (!serviceNodes.ContainsKey(iNode.ID))
        {
            ServiceNode node = MonoBehaviour.Instantiate<ServiceNode>(parent.servicePrefab, vector, Quaternion.identity);
            node.labelMesh.text = iNode.ID;
            serviceNodes.Add(iNode.ID, node);         
        }
        else
        {
            ServiceNode node = serviceNodes[iNode.ID];
            node.gameObject.SetActive(true);
            node.gameObject.transform.position = vector;
        }
    }
}

