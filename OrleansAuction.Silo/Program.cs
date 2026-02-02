using Azure.Data.Tables;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    if (builder.Environment.IsDevelopment())
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMemoryGrainStorage("auctionStore");
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("AzureStorage");

        siloBuilder.UseAzureStorageClustering(options =>
            options.TableServiceClient = new TableServiceClient(connectionString));

        siloBuilder.AddAzureTableGrainStorage("auctionStore", options =>
            options.TableServiceClient = new TableServiceClient(connectionString));
    }

    siloBuilder.UseDashboard(options =>
    {
        options.HostSelf = true;
        options.CounterUpdateIntervalMs = 1000;
    });
});

var app = builder.Build();

app.MapGet("/", () => "Silo is running...");

app.Run();