namespace OrleansAuction.Shared;

[Alias("auction-grain")]
public interface IAuctionGrain : IGrainWithStringKey
{
    [Alias("place-bid")]
    Task<bool> PlaceBid(string bidderId, decimal amount);

    [Alias("get-status")]
    Task<AuctionState> GetStatus();
}