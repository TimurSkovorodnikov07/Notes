public class NpgsqlGlobalMappers
{
    public static void MappersEnable()
    {
        Dapper.SqlMapper.AddTypeHandler(new GuidSqlMapper());
    }
    //Сколько я времени убил нахуй.. За то у меня теперь есть маппер.
}