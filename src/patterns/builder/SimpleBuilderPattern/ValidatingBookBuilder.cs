// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace SimpleBuilderPattern;

// [<snippet Builder>]
public class ValidatingBookBuilder : BookBuilder
{
    public override Book Build()
    {
        if ( this.Title == null )
        {
            throw new ValidationException( "The Title property must not be null." );
        }

        if ( this.Authors.Count == 0 )
        {
            throw new ValidationException( "There must be at least one author." );
        }

        if ( this.Tags.Contains( "Kid" ) && this.Tags.Contains( "Adult" ) )
        {
            throw new ValidationException( "The 'Kids' and 'Adults' tags are exclusive." );
        }

        return base.Build();
    }
}

// [<endsnippet Builder>]