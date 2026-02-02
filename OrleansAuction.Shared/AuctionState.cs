namespace OrleansAuction.Shared;

[GenerateSerializer]
[Alias("auction-state")]
public record AuctionState
{
    [Id(0)] public decimal CurrentBid { get; set; } = 0;

    [Id(1)] public string HighBidderId { get; set; } = "No Bids";

    [Id(2)] public bool IsActive { get; set; } = true;
}