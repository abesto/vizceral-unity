using Model.GraphLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class JSONRegion
    {
        public string renderer;
        public string name;
        public string displayName;
        public string @class;
        public float maxVolume;
        public long updated;
        public JSONService[] nodes;
        public JSONConnection[] connections;
        public RegionMetadata metadata;
    }

    public class Region: Graph<Connection<Service>, Service>
    {
        public string ID;
        public string Name;
        public Global Parent;
        private Dictionary<string, Service> ServicesByID;
        private Service[] _Nodes;
        private Connection<Service>[] _Edges;
        private ParticleRateCalculator ParticleRateCalculator;

        public Region(JSONRegion json, Global parent)
        {
            ServicesByID = new Dictionary<string, Service>();
            ParticleRateCalculator = new ParticleRateCalculator(json.maxVolume);
            Parent = parent;
            ID = json.name;
            Name = json.displayName;

            _Nodes = new Service[json.nodes.Length];
            for (int i = 0; i < json.nodes.Length; i++)
            {
                JSONService j = json.nodes[i];
                Service service = new Service(j, this);
                if (ServicesByID.ContainsKey(service.ID))
                {
                    MonoBehaviour.print("Duplicate service node with name " + service.ID);
                }
                ServicesByID[service.ID] = service;
                _Nodes[i] = service;
            }

            _Edges = new Connection<Service>[json.connections.Length];
            for (int i = 0; i < json.connections.Length; i++)
            {
                JSONConnection j = json.connections[i];
                Connection<Service> connection = new Connection<Service>(j, Service(j.source), Service(j.target));
                connection.EmitRate = ParticleRateCalculator.MetricsToEmitRate(connection.Metrics);
                _Edges[i] = connection;
            }

            
        }

        public Service Service(string serviceID)
        {
            if (!ServicesByID.ContainsKey(serviceID))
            {
                // A service from anothe region.
                // TODO: fill up with data as it becomes available. Probably by first checking on the parent whether the service data is already avaliable,
                // or otherwise listening to some event fired whenever a new service is created
                ServicesByID[serviceID] = new RemoteService(serviceID);
            }
            return ServicesByID[serviceID];
        }

        public override Service[] Nodes
        {
            get
            {
                return _Nodes;
            }

            set
            {
                _Nodes = value;
            }
        }

        
        public override Connection<Service>[] Edges
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