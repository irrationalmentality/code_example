using Newtonsoft.Json;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Code.Example.App.Services.RestApi.Logging;

public static class LoggerFactory
{
    static LoggerFactory()
    {
        SelfLog.Enable(msg =>
        {
            var data = new
            {
                date = DateTimeOffset.UtcNow,
                level = LogLevel.Error.ToString(),
                message = msg
            };
            var jsonData = JsonConvert.SerializeObject(data);

            Console.Error.WriteLine(jsonData);
        });
    }

    public static Serilog.ILogger CreateBootstrapLogger(bool logAsText = false)
    {
        var lc = new LoggerConfiguration()
            .MinimumLevel.Error()
            .Enrich.FromLogContext();

        if (!logAsText)
            lc.WriteTo.Console(new JsonFormatter());
        else
            lc.WriteTo.Console();

        return lc.CreateBootstrapLogger();
    }

    public static LoggerConfiguration ConfigureAppLogger(
        LoggerConfiguration lc,
        IConfiguration appConfiguration,
        Func<string, bool>? loggerFilter = null,
        bool logSagas = false,
        bool logAsText = false)
    {
        lc
            .MinimumLevel.Debug()
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Filter.With(new LogstashCustomFilter(loggerFilter));

        if (!logAsText)
            lc.WriteTo.Console(new JsonFormatter());
        else
            lc.WriteTo.Console();

        if (logSagas)
        {
            lc.WriteTo.SagasLogDbTable(appConfiguration);
        }

        return lc;
    }
}
