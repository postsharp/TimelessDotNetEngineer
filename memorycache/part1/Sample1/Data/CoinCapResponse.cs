namespace Sample1.Data;

public record CoinCapResponse(CoinCapData Data, long Timestamp);

public record CoinCapData(string Id, string Symbol, string CurrencySymbol, string Type, decimal RateUsd);