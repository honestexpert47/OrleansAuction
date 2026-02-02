using OrleansAuction.Shared;

namespace OrleansAuction.Silo;

public class AuctionGrain : Grain, IAuctionGrain
{
    private readonly IPersistentState<AuctionState> _state;
    private readonly ILogger<AuctionGrain> _logger;

    public AuctionGrain(
        [PersistentState("auction", "auctionStore")] IPersistentState<AuctionState> state,
        ILogger<AuctionGrain> logger)
    {
        _state = state;
        _logger = logger;
    }

    public async Task<bool> PlaceBid(string bidderId, decimal amount)
    {
        if (amount <= _state.State.CurrentBid)
        {
            _logger.LogWarning("Bid rejected. {Amount} is too low.", amount);
            return false;
        }

        _state.State.CurrentBid = amount;
        _state.State.HighBidderId = bidderId;
        _state.State.IsActive = true;

        await _state.WriteStateAsync();

        _logger.LogInformation("New High Bid! {Amount} by {Bidder}", amount, bidderId);
        return true;
    }

    public Task<AuctionState> GetStatus()
    {
        return Task.FromResult(_state.State);
    }
}