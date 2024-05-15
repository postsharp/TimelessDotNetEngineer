// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

namespace Sample1.Data;

public record CoinCapResponse( CoinCapData Data, long Timestamp );

public record CoinCapData( string Id, string Symbol, string CurrencySymbol, string Type, decimal RateUsd );