public static class RandomExtension
{
    public static int[]? NextArray(this Random random, int length, int minValue, int maxValue)
    {
        if (length <= 0)
            return null;

        var array = new int[length];

        for (int i = 0; i < length; i++)
            array[i] = random.Next(minValue, maxValue);

        return array;
    }

    public static IEnumerable<int>? NextIEnumerable(this Random random, int length, int minValue, int maxValue)
    {
        return NextArray(random, length, minValue, maxValue)?.ToList();
    }
}