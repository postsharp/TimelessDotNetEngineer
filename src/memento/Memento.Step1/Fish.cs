// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Step1;

// [<snippet Type>]
public sealed class Fish : ObservableRecipient, IMementoable
{
    private string? _name;
    private string? _species;
    private DateTime _dateAdded;

    public string? Name
    {
        get => this._name;
        set => this.SetProperty( ref this._name, value, true );
    }

    public string? Species
    {
        get => this._species;
        set => this.SetProperty( ref this._species, value, true );
    }

    public DateTime DateAdded
    {
        get => this._dateAdded;
        set => this.SetProperty( ref this._dateAdded, value, true );
    }

    public void RestoreMemento( IMemento memento )
    {
        if ( memento is not Memento s )
        {
            throw new InvalidOperationException( "Invalid snapshot." );
        }

        this.Name = s.Name;
        this.Species = s.Species;
        this.DateAdded = s.DateAdded;
    }

    public IMemento SaveToMemento()
    {
        return new Memento( this, this.Name, this.Species, this.DateAdded );
    }

    private sealed record Memento(
        Fish Originator,
        string? Name,
        string? Species,
        DateTime DateAdded ) : IMemento
    {
        IMementoable IMemento.Originator => this.Originator;
    }
}

// [<endsnippet Type>]