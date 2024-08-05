public static class ListExtension
{
    public static List<T> GetValues<T>(this List<T> list, int from, int to)
    {
        var result = new List<T>();
        for (int i = from; i < to; i++)
        {
            if (i >= list.Count)
            {
                break;
                //Нах мне заверщаьб цикл? У меня клиент короче будет не знать скок осталось, потому to = pagin
                //Допустим pagin = 10 а элементов всего 8, что будет дальше? Правильно, полный пиздец.
                //Вот, если цикл доходит до 8 го(последнего) тогда цикл закрываеться.
            }
            result.Add(list[i]);
        }
        return result;
    }
}