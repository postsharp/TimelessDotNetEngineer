using CommunityToolkit.Mvvm.ComponentModel;

namespace Memento;

public partial class ItemViewModel : ObservableRecipient, IOriginator
{
    private string? _name;
    private string? _species;
    private DateTime _dateAdded;

    public string? Name { get => this._name; set => this.SetProperty( ref this._name, value, true ); }

    public string? Species { get => this._species; set => this.SetProperty( ref this._species, value, true ); }

    public DateTime DateAdded { get => this._dateAdded; set => this.SetProperty( ref this._dateAdded, value, true ); }

    public void Restore( IMemento memento )
    {
        if ( memento is not Memento m )
        {
            throw new InvalidOperationException( "Invalid memento" );
        }

        this.Name = m.Name;
        this.Species = m.Species;
        this.DateAdded = m.DateAdded;
    }

    public IMemento Save()
    {
        return new Memento( this, this.Name, this.Species, this.DateAdded );
    }

    private class Memento : IMemento
    {
        public IOriginator Originator { get; }
        public string? Name { get; }
        public string? Species { get; }
        public DateTime DateAdded { get; }

        public Memento(ItemViewModel originator, string? name, string? species, DateTime dateAdded)
        {
            this.Originator = originator;
            this.Name = name;
            this.Species = species;
            this.DateAdded = dateAdded;
        }
    }
}
