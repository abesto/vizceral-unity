using UnityEngine;
using EpForceDirectedGraph.cs;

public class LoadSampleData : MonoBehaviour {
    ForceDirected2D physics;
    GraphRenderer graphRenderer;
    public ServiceNode servicePrefab;
    public ServiceEdge serviceEdgePrefab;

    void Start () {
        TextAsset txt = Resources.Load<TextAsset>("SampleData/sample_data");
        Model.Global global = JsonUtility.FromJson<Model.Global>(txt.text);
        Debug.Log(JsonUtility.ToJson(global));

        Graph graph = new Graph();
        physics = new ForceDirected2D(graph, 80, 50, 0.8f);
        graphRenderer = new GraphRenderer(physics, this);
        Model.Region region = global.nodes[1];
        if (region.nodes != null)
        {
            foreach (Model.Service service in region.nodes)
            {
                NodeData data = new NodeData();
                data.mass = 0;
                if (region.connections != null)
                {
                    foreach (Model.Connection connection in region.connections)
                    {
                        if (connection.source == service.name || connection.target == service.name)
                        {
                            data.mass += 1;
                        }
                    }
                }
                data.label = service.displayName;
                data.initialPostion = new FDGVector2(Random.Range(-30, 30), Random.Range(-30, 30));
                Node node = new Node(service.name, data);
                graph.AddNode(node);
            }
        }
        if (region.connections != null)
        {
            foreach (Model.Connection connection in region.connections)
            {
                EdgeData data = new EdgeData();
                data.label = connection.source + " -> " + connection.target;
                data.length = 1 / (connection.metrics.danger + connection.metrics.normal);
                graph.CreateEdge(connection.source, connection.target, data);
            };
        }
        for (var i = 0; i < 100; i++) {
            physics.Calculate(Time.fixedDeltaTime * 5);
        }
	}

    void Update()
    {
        graphRenderer.Draw(Time.deltaTime);
    }
}
