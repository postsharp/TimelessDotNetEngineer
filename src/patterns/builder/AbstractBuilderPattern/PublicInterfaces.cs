// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Immutable;

namespace BuilderPattern;

// [<snippet Interfaces>]
public interface IBook
{
    string Title { get; }

    ImmutableArray<string> Authors { get; }

    void Deliver();
}

public interface IBookBuilder
{
    string? Title { get; set; }

    List<string> Authors { get; }

    IBook Build();
}

public enum BookKind
{
    Paper = 1,
    EBook = 2
}

public interface IBookFactory
{
    IBookBuilder CreateBookBuilder( BookKind bookKind );
}

// [<endsnippet Interfaces>]