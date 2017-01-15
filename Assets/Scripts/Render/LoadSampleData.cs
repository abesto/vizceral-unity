using UnityEngine;
using Model.GraphLayout.LeftToRightTree;

public class LoadSampleData : MonoBehaviour {

    public ServiceNode servicePrefab;
    public ServiceEdge serviceEdgePrefab;

    void Start () {
        TextAsset txt = Resources.Load<TextAsset>("SampleData/sample_data");
        Model.Global global = new Model.Global(JsonUtility.FromJson<Model.JSONGlobal>(txt.text));
        
        Model.Region region = global.Region("us-east-1");

        var layouter = new LeftToRightTreeLayoutEngine<Model.Connection<Model.Service>, Model.Service>();
        layouter.SetGraph(region);

        var renderer = new RegionRenderer(region, this);
        renderer.Draw();
	}

    void Update()
    {
    }
}
