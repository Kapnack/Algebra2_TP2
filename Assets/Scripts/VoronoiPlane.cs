public struct VoronoiPlane
{
    public Plane3 plane;
    public int a;
    public int b;

    public VoronoiPlane(int a, int b, Plane3 plane)
    {
        this.a = a;
        this.b = b;
        this.plane = plane;
    }
}

public struct DelaunayEdge
{
    public int a;
    public int b;

    public DelaunayEdge(int a, int b)
    {
        this.a = a;
        this.b = b;
    }
}