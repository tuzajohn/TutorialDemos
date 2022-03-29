[assembly: FunctionsStartup(typeof(TutorialDemos.Functions.Programme))]
namespace TutorialDemos.Functions;

public class Programme : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Logentries("77d4527f-e9ee-4cad-80c4-a91b3d2a80d2")
                .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {

        });
    }

}
