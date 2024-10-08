// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Immutable;

namespace SimpleBuilderPattern;

// [<snippet Classes>]
public record Book( string Title, ImmutableArray<string> Authors, ImmutableArray<string> Tags );

public class BookBuilder
{
    public string? Title { get; set; }

    public List<string> Authors { get; set; } = new();

    public List<string> Tags { get; set; } = new();

    public virtual Book Build()
    {
        return new Book( this.Title!, [..this.Authors], [..this.Tags] );
    }
}

// [<endsnippet Classes>]