// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using Microsoft.Extensions.DependencyInjection;

namespace BuilderPattern
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            // See https://aka.ms/new-console-template for more information

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IBookFactory>( new BookBuilderFactory() );
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // [<snippet Use>]
            var factory = serviceProvider.GetRequiredService<IBookFactory>();
            var builder = factory.CreateBookBuilder( BookKind.Paper );
            builder.Title = "Dix contes de Perrault";
            builder.Authors.Add( "Charles Perrault" );
            var book = builder.Build();
            book.Deliver();

            // [<endsnippet Use>]
        }
    }
}