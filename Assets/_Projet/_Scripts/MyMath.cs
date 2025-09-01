using UnityEngine;

public struct MyMath
{
    public static int Modulo(int index, int count)
    {
        if (count == 0) return 0;
        return (index % count + count) % count;
    }

    public static bool IsPair(int n)
    {
        return n % 2 == 0;
    }
}
