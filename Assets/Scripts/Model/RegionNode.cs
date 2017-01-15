using Model.GraphLayout;


namespace Model
{
    public class RegionNode: Node
    {
        public Region Region;

        public RegionNode(Region region)
        {
            Region = region;
        }
    }
}
