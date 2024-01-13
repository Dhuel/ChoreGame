using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;
using TheBoops.Database.DbHandlers;
using TheBoops.ViewModel;

namespace TheBoops
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            //Setup DynamoDB Client
            var region = RegionEndpoint.USWest2;
            var credentials = new BasicAWSCredentials(
                        accessKey: "{Access Key}",
                        secretKey: "{Secret Key}");
            builder.Services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(credentials, region));

            //Regitering the repository
            builder.Services.AddSingleton<IDbHandler, DbHandler>();

            //Register ViewModels and Pages
            builder.Services.AddTransient<UsersPage>();
            builder.Services.AddTransient<UsersPageViewModel>();

            return builder.Build();
        }
    }
}
