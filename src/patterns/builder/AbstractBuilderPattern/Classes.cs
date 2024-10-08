// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Immutable;

namespace BuilderPattern;

// [<snippet ImmutableClasses>]
internal abstract record Book( int Id, string Title, ImmutableArray<string> Authors ) : IBook
{
    public abstract void Deliver();
}

internal record PaperBook( int Id, string Title, ImmutableArray<string> Authors )
    : Book( Id, Title, Authors )
{
    public override void Deliver() => throw new NotImplementedException();
}

internal record EBook( int Id, string Title, ImmutableArray<string> Authors )
    : Book( Id, Title, Authors )
{
    public override void Deliver() => throw new NotImplementedException();
}

// [<endsnippet ImmutableClasses>]

// [<snippet Builders>]
internal abstract class BookBuilder : IBookBuilder
{
    public int Id { get; }

    public string? Title { get; set; }

    public List<string> Authors { get; } = new();

    protected BookBuilder( int id )
    {
        this.Id = id;
    }

    public abstract IBook Build();
}

internal class PaperBookBuilder : BookBuilder
{
    public PaperBookBuilder( int id ) : base( id ) { }

    public override IBook Build() => new PaperBook( this.Id, this.Title!, [..this.Authors] );
}

internal class EBookBuilder : BookBuilder
{
    public EBookBuilder( int id ) : base( id ) { }

    public override IBook Build() => new EBook( this.Id, this.Title!, [..this.Authors] );
}

// [<endsnippet Builders>]

public interface IIdGenerator
{
    int Next();
}

// [<snippet Factory>]
internal class BookBuilderFactory( IIdGenerator idGenerator ) : IBookFactory
{
    public IBookBuilder CreateBookBuilder( BookKind kind )
    {
        var id = idGenerator.Next();

        return kind switch
        {
            BookKind.Paper => new PaperBookBuilder( id ),
            BookKind.EBook => new EBookBuilder( id ),
            _ => throw new ArgumentException()
        };
    }
}

// [<endsnippet Factory>]