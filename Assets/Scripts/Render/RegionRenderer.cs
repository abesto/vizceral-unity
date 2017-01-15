using Model;
using System.Collections.Generic;
using UnityEngine;

public class RegionRenderer
{
    LoadSampleData parent;
    Dictionary<string, ServiceNode> serviceNodes;
    Dictionary<string, ServiceEdge> serviceEdges;
    Region region;

    public RegionRenderer(Region region, LoadSampleData parent)
    {
        this.parent = parent;
        this.serviceNodes = new Dictionary<string, ServiceNode>();
        this.serviceEdges = new Dictionary<string, ServiceEdge>();
        this.region = region;
    }

    protected void drawEdge(Connection<Service> edge)
    {
        if (serviceNodes.ContainsKey(edge.Source.ID) && serviceNodes.ContainsKey(edge.Target.ID))
        {
            if (!serviceEdges.ContainsKey(edge.ID))
            {
                serviceEdges.Add(edge.ID, MonoBehaviour.Instantiate<ServiceEdge>(parent.serviceEdgePrefab, Vector2.zero, Quaternion.identity));
            }
            serviceEdges[edge.ID].UpdateData(serviceNodes[edge.Source.ID], serviceNodes[edge.Target.ID], edge.EmitRate);
        }
    }

    protected void drawNode(Service service)
    {
        if (!serviceNodes.ContainsKey(service.ID))
        {
            ServiceNode node = MonoBehaviour.Instantiate<ServiceNode>(parent.servicePrefab, service.Position, Quaternion.identity);
            node.labelMesh.text = service.ID;
            serviceNodes.Add(service.ID, node);         
        }
        else
        {
            ServiceNode node = serviceNodes[service.ID];
            node.gameObject.SetActive(true);
            node.gameObject.transform.position = service.Position;
        }
    }

    public void Draw()
    {
        foreach (Service service in region.Nodes)
        {
            drawNode(service);
        }
        foreach (Connection<Service> edge in region.Edges)
        {
            drawEdge(edge);
        }
    }
}
