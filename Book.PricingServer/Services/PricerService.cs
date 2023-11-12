using Book.PricingEngine;
using Grpc.Core;
namespace Book.PricingServer.Services;

public class PricerService : Pricer.PricerBase {
    private readonly ILogger<PricerService> logger;
    public PricerService(ILogger<PricerService> logger) {
        this.logger = logger;
    }

    public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
        return Task.FromResult(new PriceReply() { CurrencyCode = "RUB", Price = 555 });
    }
}