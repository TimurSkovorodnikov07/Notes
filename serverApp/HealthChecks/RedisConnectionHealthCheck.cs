using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

public class RedisConnectionHealthCheck : IHealthCheck
{
    private readonly ILogger<RedisConnectionHealthCheck> _logger;
    private readonly HealthOptions _healthOptions;
    private readonly string _connectionStr;

    public RedisConnectionHealthCheck(ILogger<RedisConnectionHealthCheck> logger, IOptions<HealthOptions> options, IConfiguration conf)
    {
        _logger = logger;
        _healthOptions = options.Value;
        _connectionStr = conf["UserSecrets:RedisConnectionStr"];
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var _connectionInfo = ConnectionMultiplexer.Connect(_connectionStr);
        var ping = await _connectionInfo.GetDatabase().PingAsync();

        var seconds = ping.Seconds;
        if (seconds > _healthOptions.ConnectionTakesNoMoreSeconds)
        {
            return HealthCheckResult.Degraded($"Connection is OK. Connected for: {seconds}");
        }
        else
        {
            return HealthCheckResult.Healthy($"Connection is OK. Connected for: {seconds}");
        }
        //Я не вижу смысла писать сюда если не подкл = unhealthy,
        //потому что если не подкл приложение само по себе не будет работать коректно, тут улчше exseptions всякие будут
    }
}