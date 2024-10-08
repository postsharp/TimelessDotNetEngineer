// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace AsyncBuilderPattern;

// [<snippet Example>]
public record Book( int Id, string Title );

internal interface IDatabaseIdGenerator
{
    Task<int> NextIdAsync();
}

public class BookBuilder
{
    private readonly IDatabaseIdGenerator _idGenerator;

    internal BookBuilder( IDatabaseIdGenerator idGenerator )
    {
        this._idGenerator = idGenerator;
    }

    public string? Title { get; set; }

    public async Task<Book> BuildAsync()
    {
        var id = await this._idGenerator.NextIdAsync();

        return new Book( id, this.Title! );
    }
}

// [<endsnippet Example>]