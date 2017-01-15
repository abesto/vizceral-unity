namespace Model.GraphLayout
{
    interface LayoutEngine<E, N> where N: Node where E: Edge<N>
    {
        void SetGraph(Graph<E, N> graph);
        void Update(float deltaTime);
    }
}
