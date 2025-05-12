using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace Code.Example.App.Services.RestApi;

public static class Program
{
    private static bool LogAsText;

    public static int Main(string[] args)
    {
        LogAsText = false;
#if DEBUG
        LogAsText = true;
        Console.Title = "RestApi";
#endif

        Log.Logger = LoggerFactory.CreateBootstrapLogger(logAsText: LogAsText);

        var runId = Guid.NewGuid().ToString();

        try
        {
            Log.Information("Starting the API service. RunID: {runId}", runId);

            CreateHostBuilder(args).Build().Run();

            Log.Information("The API service has been stopped. RunID: {runId}", runId);

            return Environment.ExitCode;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The API service terminated unexpectedly. RunID: {runId}", runId);
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureAppConfiguration((_, config) =>
            {
                AddAppConfiguration(config, args);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog((context, services, lc) => LoggerFactory.ConfigureAppLogger(lc, context.Configuration, logAsText: LogAsText));

    private static void AddAppConfiguration(this IConfigurationBuilder config, string[] args)
    {
        var imageUri = new Uri(Assembly.GetExecutingAssembly().Location);
        var currentDir = Path.GetDirectoryName(imageUri.LocalPath)!;
        var configPath = Path.Combine(currentDir, "vstack.config.yaml");
        config.AddYamlFile(configPath, optional: false, reloadOnChange: true);

        config.AddEnvironmentVariables(prefix: "VSTACK_");
        if (args.Length > 0)
        {
            config.AddCommandLine(args);
        }
    }
}
