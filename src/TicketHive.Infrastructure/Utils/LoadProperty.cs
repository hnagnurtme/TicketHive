using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TicketHive.Infrastructure.Utils;

public class LoadProperty
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoadProperty> _logger;

    public LoadProperty(IConfiguration configuration, ILogger<LoadProperty> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Lấy property theo key (instance method).
    /// Có log thông tin khi lấy.
    /// </summary>
    public string Get(string key)
    {
        var value = _configuration[key];
        if (string.IsNullOrEmpty(value))
        {
            _logger.LogWarning("Config key '{Key}' not found or empty.", key);
        }
        else
        {
            _logger.LogInformation("Loaded config key '{Key}' = '{Value}'", key, value);
        }

        return value ?? string.Empty;
    }

    /// <summary>
    /// Lấy property theo key (static method).
    /// In log ra console (không có ILogger).
    /// </summary>
    public static string GetProperty(IConfiguration configuration, string key)
    {
        var value = configuration[key];
        if (string.IsNullOrEmpty(value))
        {
            Console.WriteLine($"[LoadProperty] Config key '{key}' not found or empty.");
        }
        else
        {
            Console.WriteLine($"[LoadProperty] Loaded config key '{key}' = '{value}'");
        }

        return value ?? string.Empty;
    }
}
