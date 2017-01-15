using Model.GraphLayout;
using System;

namespace Model
{
    [Serializable]
    public class JSONService
    {
        public string renderer;  // Always "focusedChild"
        public string name;
        public string displayName;
        public string @class;
        public long updated;
        public JSONCluster[] clusters;
    }

    public class Service: Node {
        public Region Region;
        public string Name;
        public string @Class;
        public long Updated;

        public Service()
        {

        }

        public Service(JSONService json, Region region)
        {
            Region = region;
            ID = json.name;
            Name = json.displayName;
            Class = json.@class;
            Updated = json.updated;
        }
    }

    public class RemoteService : Service
    {
        public RemoteService(string ID) : base()
        {
            this.ID = ID;
        }
    }
}