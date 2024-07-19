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
        get => _name;
        set => SetProperty( ref _name, value, true );
    }

    public string? Species
    {
        get => _species;
        set => SetProperty( ref _species, value, true );
    }

    public DateTime DateAdded
    {
        get => _dateAdded;
        set => SetProperty( ref _dateAdded, value, true );
    }
}

// [<endsnippet Type>]