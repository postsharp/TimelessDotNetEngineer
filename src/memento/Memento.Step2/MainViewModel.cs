using System.Collections.Immutable;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Metalama.Patterns.Observability;
using Metalama.Patterns.Wpf;
using NameGenerator.Generators;

namespace Memento.Step2;

[Memento]
[Observable]
public sealed partial class MainViewModel : ObservableRecipient
{
    private ISnapshotCaretaker? _caretaker;
    private readonly IFishGenerator _fishGenerator;

    public bool IsEditing { get; set; }

    public ImmutableList<Fish> Fishes { get; private set; } = ImmutableList<Fish>.Empty;

    public Fish? CurrentFish { get; set; }

    // Design-time.
    public MainViewModel() : this( new FishGenerator( new RealNameGenerator() ), null ) { }

    public MainViewModel( IFishGenerator fishGenerator, ISnapshotCaretaker? caretaker )
    {
        _fishGenerator = fishGenerator;
        _caretaker = caretaker;
    }

    [Command]
    private void ExecuteNew()
    {
        _caretaker?.CaptureSnapshot( this );

        Fishes = Fishes.Add( new Fish() { Name = _fishGenerator.GetNewName(), Species = _fishGenerator.GetNewSpecies(), DateAdded = DateTime.Now } );
    }

    public bool CanExecuteNew => !IsEditing;

    [Command]
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

    public bool CanExecuteRemove => CurrentFish != null && !IsEditing;

    [Command]
    private void ExecuteEdit()
    {
        IsEditing = true;
        _caretaker?.CaptureSnapshot( CurrentFish! );
    }

    public bool CanExecuteEdit => CurrentFish != null && !IsEditing;

    [Command]
    private void ExecuteSave()
    {
        IsEditing = false;
    }

    public bool CanExecuteSave => IsEditing;

    [Command]
    private void ExecuteCancel()
    {
        IsEditing = false;
        _caretaker?.Undo();
    }

    public bool CanExecuteCancel => IsEditing;

    [Command]
    private void ExecuteUndo()
    {
        IsEditing = false;

        // Remember the main list selection status before undo.
        var item = CurrentFish;

        var index =
            item != null
                ? (int?)Fishes.IndexOf( item )
                : null;

        _caretaker?.Undo();

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
    }

    public bool CanExecuteUndo => _caretaker?.CanUndo == true;
}