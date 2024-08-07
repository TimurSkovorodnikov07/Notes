public static class GuidExtensions
{
    // Метод расширения для сравнения Guid и string
    public static bool Equals(this Guid guid, string str)
    {
        return guid.ToString() == str;
    }

    public static bool NotEquals(this Guid guid, string str)
    {
        return guid.ToString() != str;
    }
}