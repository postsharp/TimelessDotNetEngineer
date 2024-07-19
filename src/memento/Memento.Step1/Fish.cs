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

    public void RestoreMemento( IMemento memento )
    {
        if (memento is not Memento s)
        {
            throw new InvalidOperationException( "Invalid snapshot." );
        }

        Name = s.Name;
        Species = s.Species;
        DateAdded = s.DateAdded;
    }

    public IMemento SaveToMemento()
    {
        return new Memento( this, Name, Species, DateAdded );
    }

    private sealed record Memento( Fish Originator, string? Name, string? Species, DateTime DateAdded ) : IMemento
    {
        IMementoable IMemento.Originator => Originator;
    }
}

// [<endsnippet Type>]