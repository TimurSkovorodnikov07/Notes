public class CodeService : ICodeCreator
{
    public string? Create(int length)
    {
        var list = new Random().NextIEnumerable(length, 0, 9);
        if (list is not null)
        {
            return string.Join("", list);
        }
        return null;
    }
}