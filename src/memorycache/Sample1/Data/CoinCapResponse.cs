// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace Sample1.Data;

public record CoinCapResponse( CoinCapData Data, long Timestamp );

public record CoinCapData(
    string Id,
    string Symbol,
    string CurrencySymbol,
    string Type,
    decimal RateUsd );