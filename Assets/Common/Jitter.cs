public static class Jitter
{
    public static float Halton(int index, int baseValue)
    {
        var result = 0f;
        var f = 1f / baseValue;
        var i = index;
        while (i > 0)
        {
            result += f * (i % baseValue);
            i /= baseValue;
            f /= baseValue;
        }
        return result;
    }
}
