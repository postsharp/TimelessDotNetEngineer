// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

using System.Collections.Immutable;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NameGenerator.Generators;

namespace Memento.Step1;

public sealed class MainViewModel : ObservableRecipient, IMementoable
{
    // [<snippet DataFields>]
    private readonly IMementoCaretaker? _caretaker;
    private readonly IFishGenerator _fishGenerator;
    private bool _isEditing;
    private Fish? _currentFish;
    private ImmutableList<Fish> _fishes = ImmutableList<Fish>.Empty;

    // [<endsnippet DataFields>]

    public IRelayCommand NewCommand { get; }

    public IRelayCommand RemoveCommand { get; }

    public IRelayCommand EditCommand { get; }

    public IRelayCommand SaveCommand { get; }

    public IRelayCommand CancelCommand { get; }

    public IRelayCommand UndoCommand { get; }

    public bool IsEditing
    {
        get => this._isEditing;
        set => this.SetProperty( ref this._isEditing, value, true );
    }

    public ImmutableList<Fish> Fishes
    {
        get => this._fishes;
        private set => this.SetProperty( ref this._fishes, value, true );
    }

    public Fish? CurrentFish
    {
        get => this._currentFish;
        set => this.SetProperty( ref this._currentFish, value, true );
    }

    // Design-time.
    public MainViewModel() : this( new FishGenerator( new RealNameGenerator() ), null ) { }

    public MainViewModel( IFishGenerator fishGenerator, IMementoCaretaker? caretaker )
    {
        this._fishGenerator = fishGenerator;
        this._caretaker = caretaker;

        this.NewCommand = new RelayCommand( this.ExecuteNew, this.CanExecuteNew );
        this.RemoveCommand = new RelayCommand( this.ExecuteRemove, this.CanExecuteRemove );
        this.EditCommand = new RelayCommand( this.ExecuteEdit, this.CanExecuteEdit );
        this.SaveCommand = new RelayCommand( this.ExecuteSave, this.CanExecuteSave );
        this.CancelCommand = new RelayCommand( this.ExecuteCancel, this.CanExecuteCancel );
        this.UndoCommand = new RelayCommand( this.ExecuteUndo, this.CanExecuteUndo );
    }

    protected override void OnPropertyChanged( PropertyChangedEventArgs e )
    {
        switch ( e.PropertyName )
        {
            case nameof(this.IsEditing):
                this.NewCommand.NotifyCanExecuteChanged();
                this.RemoveCommand.NotifyCanExecuteChanged();
                this.EditCommand.NotifyCanExecuteChanged();
                this.SaveCommand.NotifyCanExecuteChanged();
                this.CancelCommand.NotifyCanExecuteChanged();
                this.UndoCommand.NotifyCanExecuteChanged();

                break;

            case nameof(this.CurrentFish):
                this.EditCommand.NotifyCanExecuteChanged();
                this.RemoveCommand.NotifyCanExecuteChanged();
                this.UndoCommand.NotifyCanExecuteChanged();

                break;

            case nameof(this.Fishes):
                this.UndoCommand.NotifyCanExecuteChanged();

                break;
        }

        base.OnPropertyChanged( e );
    }

    // [<snippet ExecuteNew>]
    private void ExecuteNew()
    {
        this._caretaker?.CaptureSnapshot( this );

        this.Fishes = this.Fishes.Add(
            new Fish()
            {
                Name = this._fishGenerator.GetNewName(),
                Species = this._fishGenerator.GetNewSpecies(),
                DateAdded = DateTime.Now
            } );
    }

    // [<endsnippet ExecuteNew>]

    private bool CanExecuteNew()
    {
        return !this.IsEditing;
    }

    private void ExecuteRemove()
    {
        if ( this.CurrentFish != null )
        {
            this._caretaker?.CaptureSnapshot( this );

            var index = this.Fishes.IndexOf( this.CurrentFish );
            this.Fishes = this.Fishes.RemoveAt( index );

            if ( index < this.Fishes.Count )
            {
                this.CurrentFish = this.Fishes[index];
            }
            else if ( this.Fishes.Count > 0 )
            {
                this.CurrentFish = this.Fishes[^1];
            }
            else
            {
                this.CurrentFish = null;
            }
        }
    }

    private bool CanExecuteRemove()
    {
        return this.CurrentFish != null && !this.IsEditing;
    }

    // [<snippet EditControl>]
    private void ExecuteEdit()
    {
        this.IsEditing = true;

        this._caretaker?.CaptureSnapshot( this._currentFish! );
    }

    private void ExecuteSave()
    {
        this.IsEditing = false;
    }

    private void ExecuteCancel()
    {
        this.IsEditing = false;
        this._caretaker?.Undo();
    }

    // [<endsnippet EditControl>]

    private bool CanExecuteCancel()
    {
        return this.IsEditing;
    }

    private bool CanExecuteSave()
    {
        return this.IsEditing;
    }

    private bool CanExecuteEdit()
    {
        return this.CurrentFish != null && !this.IsEditing;
    }

    // [<snippet ExecuteUndo>]
    private void ExecuteUndo()
    {
        this.IsEditing = false;

        // Remember the main list selection status before undo.
        var item = this.CurrentFish;

        var index =
            item != null
                ? (int?) this.Fishes.IndexOf( item )
                : null;

        this._caretaker!.Undo();

        // Fix the current item after undo.
        if ( index != null )
        {
            if ( index < this.Fishes.Count )
            {
                this.CurrentFish = this.Fishes[index.Value];
            }
            else if ( this.Fishes.Count > 0 )
            {
                this.CurrentFish = this.Fishes[^1];
            }
            else
            {
                this.CurrentFish = null;
            }
        }

        this.UndoCommand.NotifyCanExecuteChanged();
    }

    // [<endsnippet ExecuteUndo>]

    private bool CanExecuteUndo()
    {
        return this._caretaker is { CanUndo: true };
    }

    public IMemento SaveToMemento()
    {
        return new Memento( this, this.IsEditing, this.CurrentFish, this.Fishes );
    }

    public void RestoreMemento( IMemento memento )
    {
        if ( memento is not Memento m )
        {
            throw new InvalidOperationException( "Invalid memento" );
        }

        this.IsEditing = m.IsEditing;
        this.CurrentFish = m.CurrentItem;
        this.Fishes = m.Items;
    }

    // [<snippet Memento>]
    private class Memento : IMemento
    {
        public IMementoable Originator { get; }

        public bool IsEditing { get; }

        public Fish? CurrentItem { get; }

        public ImmutableList<Fish> Items { get; }

        public Memento(
            MainViewModel snapshotable,
            bool isEditing,
            Fish? currentItem,
            ImmutableList<Fish> items )
        {
            this.Originator = snapshotable;
            this.IsEditing = isEditing;
            this.CurrentItem = currentItem;
            this.Items = items;
        }
    }

    // [<endsnippet Memento>]
}