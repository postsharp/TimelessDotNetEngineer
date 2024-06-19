// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Mediator;
using Microsoft.Extensions.DependencyInjection;

// Initialize services.
var services = new ServiceCollection();

services.AddTransient( sp => new Lazy<IWarehouse>( sp.GetRequiredService<IWarehouse> ) );
services.AddSingleton<IWarehouse, Warehouse>();
services.AddSingleton<IMetricService, MetricService>();
services.AddSingleton<ILoggingService, LoggingService>();
services.AddSingleton<IDistributionMediator, DistributionMediator>();
services.AddSingleton<IReturnMediator, ReturnMediator>();

var serviceProvider = services.BuildServiceProvider();

// Create instances of the classes.
var w = serviceProvider.GetRequiredService<IWarehouse>();
var distributionMediator = serviceProvider.GetRequiredService<IDistributionMediator>();
var s1 = ActivatorUtilities.CreateInstance<Store>( serviceProvider, "store1" );
var s2 = ActivatorUtilities.CreateInstance<Store>( serviceProvider, "store2" );

var item1 = new Item { Kind = "item1" };
var item2 = new Item { Kind = "item2" };

// Add stores.
distributionMediator.AddStore( s1 );
distributionMediator.AddStore( s2 );

// Run the scenario.
w.DistributeItem( item1 );
w.DistributeItem( item2 );
s1.ReturnItem( s1.Items[0] );

// Print metrics.
serviceProvider.GetRequiredService<IMetricService>().Print();
