using System;

public struct GridCoordinates
{
    public int X;
    public int Y;
    
    public bool Equals(GridCoordinates other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        return obj is GridCoordinates other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(GridCoordinates g1, GridCoordinates g2)
    {
        return g1.X == g2.X && g1.Y == g2.Y;
    }

    public static bool operator !=(GridCoordinates g1, GridCoordinates g2)
    {
        return !(g1 == g2);
    }

    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
}