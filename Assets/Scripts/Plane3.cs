using System;


[Serializable]
public struct Plane3
{
    public Vec3 normal;
    public float d;

    public static Plane3 BisectorPlane(Vec3 a, Vec3 b)
    {
        Vec3 dir = b - a;
        Vec3 mid = new Vec3((a + b) * 0.5f);
        return new Plane3(dir, mid);
    }

    public Plane3(Vec3 normal, Vec3 pointOnPlane)
    {
        this.normal = normal.normalized;
        d = Vec3.Dot(this.normal, pointOnPlane);
    }

    public float SignedDistance(Vec3 p) => Vec3.Dot(normal, p) - d;

}