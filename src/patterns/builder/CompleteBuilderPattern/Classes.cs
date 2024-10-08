// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace CompleteBuilderPattern;

// [<snippet ImmutableClasses>]
public partial record Book( string Title, ImmutableArray<string> Authors );

public partial record EBook( string Title, ImmutableArray<string> Authors, string Url )
    : Book( Title, Authors );

// [<endsnippet ImmutableClasses>]

// [<snippet BookBuilder>]
public partial record Book
{
    public virtual Builder ToBuilder() => new( this );

    public class Builder
    {
        public string? Title { get; set; }

        public ImmutableArray<string>.Builder Authors { get; set; }

        public Builder()
        {
            this.Authors = ImmutableArray.CreateBuilder<string>();
        }

        public Builder( Book prototype )
        {
            this.Title = prototype.Title;
            this.Authors = prototype.Authors.ToBuilder();
        }

        protected virtual void Validate()
        {
            if ( this.Title == null )
            {
                throw new ValidationException( "The book title cannot be null." );
            }

            if ( this.Authors.Count == 0 )
            {
                throw new ValidationException( "The book author cannot be null." );
            }
        }

        protected virtual Book Build()
        {
            this.Validate();

            return new Book( this.Title!, this.Authors.ToImmutableArray() );
        }
    }
}

// [<endsnippet BookBuilder>]

// [<snippet EBookBuilder>]
public partial record EBook
{
    public override Builder ToBuilder() => new( this );

    public new class Builder : Book.Builder
    {
        public string? Url { get; set; }

        public Builder() { }

        public Builder( EBook prototype ) : base( prototype )
        {
            this.Url = prototype.Url;
        }

        protected override void Validate()
        {
            base.Validate();

            if ( this.Url == null )
            {
                throw new ValidationException( "The book Url cannot be null." );
            }
        }

        protected override EBook Build()
        {
            base.Validate();

            return new EBook( this.Title!, this.Authors.ToImmutableArray(), this.Url! );
        }
    }
}

// [<endsnippet EBookBuilder>]