using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using OrleansAuction.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleansClient(client =>
{
    if (builder.Environment.IsDevelopment())
    {
        client.UseLocalhostClustering();
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("AzureStorage");

        client.UseAzureStorageClustering(options =>
            options.TableServiceClient = new TableServiceClient(connectionString));
    }
});

var app = builder.Build();

app.MapGet("/auction/{id}", async (IClusterClient client, string id) =>
{
    var grain = client.GetGrain<IAuctionGrain>(id);

    var status = await grain.GetStatus();

    return Results.Ok(status);
});

app.MapPost("/auction/{id}/bid", async (IClusterClient client, string id, [FromQuery] decimal amount, [FromQuery] string user) =>
{
    var grain = client.GetGrain<IAuctionGrain>(id);

    var success = await grain.PlaceBid(user, amount);

    if (!success)
    {
        return Results.BadRequest(new { Message = "Bid Rejected. Price too low." });
    }

    return Results.Ok(new { Message = "Bid Accepted!", Amount = amount });
});

app.Run();