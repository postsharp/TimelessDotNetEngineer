// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

namespace SimpleBuilderPattern;

// [<snippet TaggingService>]
public class TaggingService
{
    private readonly List<Action<BookBuilder>> _taggers = new();

    public void RegisterTagger( Action<BookBuilder> tagger ) => this._taggers.Add( tagger );

    public void Tag( BookBuilder bookBuilder )
    {
        foreach ( var tagger in this._taggers )
        {
            tagger.Invoke( bookBuilder );
        }
    }
}

// [<endsnippet TaggingService>]