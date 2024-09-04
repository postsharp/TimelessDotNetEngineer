// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento.Step0;

// [<snippet Type>]
public sealed class Fish : ObservableRecipient
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
}

// [<endsnippet Type>]