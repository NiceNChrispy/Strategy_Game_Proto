using DataStructures;

public struct HexHeuristic : IHeuristic<Hex>
{
    public float Heuristic(Hex from, Hex to)
    {
        return from.Distance(to);
    }

    public float NeighborDistance(Hex from, Hex to)
    {
        return 1.0f;
    }
}
