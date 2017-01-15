using System;

namespace Model
{

    [Serializable]
    public class Region
    {
        public string renderer;  // Always "region"
        public string name;
        public string displayName;
        public string @class;
        public long maxVolume;
        public long updated;
        public Service[] nodes;
        public Connection[] connections;
        public RegionMetadata metadata;
    }
}