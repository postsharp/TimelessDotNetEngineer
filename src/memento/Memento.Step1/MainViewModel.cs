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
        get => _isEditing;
        set => SetProperty( ref _isEditing, value, true );
    }

    public ImmutableList<Fish> Fishes
    {
        get => _fishes;
        private set => SetProperty( ref _fishes, value, true );
    }

    public Fish? CurrentFish
    {
        get => _currentFish;
        set => SetProperty( ref _currentFish, value, true );
    }

    // Design-time.
    public MainViewModel() : this( new FishGenerator( new RealNameGenerator() ), null ) { }

    public MainViewModel( IFishGenerator fishGenerator, IMementoCaretaker? caretaker )
    {
        _fishGenerator = fishGenerator;
        _caretaker = caretaker;

        NewCommand = new RelayCommand( ExecuteNew, CanExecuteNew );
        RemoveCommand = new RelayCommand( ExecuteRemove, CanExecuteRemove );
        EditCommand = new RelayCommand( ExecuteEdit, CanExecuteEdit );
        SaveCommand = new RelayCommand( ExecuteSave, CanExecuteSave );
        CancelCommand = new RelayCommand( ExecuteCancel, CanExecuteCancel );
        UndoCommand = new RelayCommand( ExecuteUndo, CanExecuteUndo );
    }

    protected override void OnPropertyChanged( PropertyChangedEventArgs e )
    {
        if (e.PropertyName == nameof(IsEditing))
        {
            NewCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
            EditCommand.NotifyCanExecuteChanged();
            SaveCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
            UndoCommand.NotifyCanExecuteChanged();
        }
        else if (e.PropertyName == nameof(CurrentFish))
        {
            EditCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
            UndoCommand.NotifyCanExecuteChanged();
        }
        else if (e.PropertyName == nameof(Fishes))
        {
            UndoCommand.NotifyCanExecuteChanged();
        }

        base.OnPropertyChanged( e );
    }

    // [<snippet ExecuteNew>]
    private void ExecuteNew()
    {
        _caretaker?.CaptureSnapshot( this );

        Fishes = Fishes.Add( new Fish() { Name = _fishGenerator.GetNewName(), Species = _fishGenerator.GetNewSpecies(), DateAdded = DateTime.Now } );
    }

    // [<endsnippet ExecuteNew>]

    private bool CanExecuteNew()
    {
        return !IsEditing;
    }

    private void ExecuteRemove()
    {
        if (CurrentFish != null)
        {
            _caretaker?.CaptureSnapshot( this );

            var index = Fishes.IndexOf( CurrentFish );
            Fishes = Fishes.RemoveAt( index );

            if (index < Fishes.Count)
            {
                CurrentFish = Fishes[index];
            }
            else if (Fishes.Count > 0)
            {
                CurrentFish = Fishes[^1];
            }
            else
            {
                CurrentFish = null;
            }
        }
    }

    private bool CanExecuteRemove()
    {
        return CurrentFish != null && !IsEditing;
    }

    // <snippet EditControl>
    private void ExecuteEdit()
    {
        IsEditing = true;

        _caretaker?.CaptureSnapshot( _currentFish! );
    }

    private void ExecuteSave()
    {
        IsEditing = false;
    }

    private void ExecuteCancel()
    {
        IsEditing = false;
        _caretaker?.Undo();
    }

    // <endsnippet EditControl>

    private bool CanExecuteCancel()
    {
        return IsEditing;
    }

    private bool CanExecuteSave()
    {
        return IsEditing;
    }

    private bool CanExecuteEdit()
    {
        return CurrentFish != null && !IsEditing;
    }

    // [<snippet ExecuteUndo>]
    private void ExecuteUndo()
    {
        IsEditing = false;

        // Remember the main list selection status before undo.
        var item = CurrentFish;

        var index =
            item != null
                ? (int?)Fishes.IndexOf( item )
                : null;

        _caretaker!.Undo();

        // Fix the current item after undo.
        if (index != null)
        {
            if (index < Fishes.Count)
            {
                CurrentFish = Fishes[index.Value];
            }
            else if (Fishes.Count > 0)
            {
                CurrentFish = Fishes[^1];
            }
            else
            {
                CurrentFish = null;
            }
        }

        UndoCommand.NotifyCanExecuteChanged();
    }

    // [<endsnippet ExecuteUndo>]

    private bool CanExecuteUndo()
    {
        return _caretaker is { CanUndo: true };
    }

    public IMemento SaveToMemento()
    {
        return new Memento( this, IsEditing, CurrentFish, Fishes );
    }

    public void RestoreMemento( IMemento memento )
    {
        if (memento is not Memento m)
        {
            throw new InvalidOperationException( "Invalid memento" );
        }

        IsEditing = m.IsEditing;
        CurrentFish = m.CurrentItem;
        Fishes = m.Items;
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
            Originator = snapshotable;
            IsEditing = isEditing;
            CurrentItem = currentItem;
            Items = items;
        }
    }

    // [<endsnippet Memento>]
}