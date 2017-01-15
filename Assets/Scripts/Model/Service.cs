using System;

namespace Model
{
    [Serializable]
    public class Service
    {
        public string renderer;  // Always "focusedChild"
        public string name;
        public string displayName;
        public string @class;
        public long updated;
        public Cluster[] clusters;
    }
}