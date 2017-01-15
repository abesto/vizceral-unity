using System;
using System.Collections.Generic;
using Model.GraphLayout;
using System.Linq;

namespace Model
{
    [Serializable]
    public class JSONGlobal
    {
        // JSON Fields
        public string renderer;
        public string name;
        public long serverUpdateTime;
        public JSONRegion[] nodes;
        public JSONConnection[] connections;
        // EOF JSON Fields


        public Global Inflate()
        {
            return new Global(this);
        }
    }

    public class Global: Graph<Connection<RegionNode>, RegionNode>
    {
        public string Name;
        public long ServerUpdateTime;
        private Region[] _Regions;
        private Connection<RegionNode>[] _Edges;
        private Dictionary<string, Region> RegionsByID;
        private Dictionary<string, RegionNode> RegionNodesByID;

        public Global(JSONGlobal json)
        {
            RegionsByID = new Dictionary<string, Region>();
            RegionNodesByID = new Dictionary<string, RegionNode>();
            Name = json.name;

            _Regions = new Region[json.nodes.Length];
            for (int i = 0; i < json.nodes.Length; i++)
            {
                JSONRegion j = json.nodes[i];
                Region region = new Region(j, this);
                if (RegionsByID.ContainsKey(region.ID))
                {
                    throw new ApplicationException("Duplicate region node with name " + region.ID);
                }
                RegionsByID[region.ID] = region;
                RegionNodesByID[region.ID] = new RegionNode(region);
                _Regions[i] = region;
            }

            _Edges = json.connections.Select(j => new Connection<RegionNode>(j, RegionNode(j.source), RegionNode(j.target))).ToArray();
        }

        public Region Region(string ID)
        {
            return RegionsByID[ID];
        }

        public RegionNode RegionNode(string ID)
        {
            return RegionNodesByID[ID];
        }

        public override RegionNode[] Nodes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override Connection<RegionNode>[] Edges
        {
            get
            {
                return _Edges;
            }

            set
            {
                _Edges = value;
            }
        }
    }
}