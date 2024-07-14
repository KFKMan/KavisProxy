namespace KavisProxy.Core.Protocol;

public struct Vec3i
{
    public Vec3i(int x, int y, int z)
    {
        X = x;
        Y = y; 
        Z = z;
    }

    public int X, Y, Z;
}