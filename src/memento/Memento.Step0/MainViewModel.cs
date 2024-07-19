using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NameGenerator.Generators;

namespace Memento.Step0;

#pragma warning disable IDE0032 // Use auto property

public sealed class MainViewModel : ObservableRecipient
{
    // [<snippet DataFields>]
    private readonly IFishGenerator _fishGenerator;
    private bool _isEditing;
    private Fish? _currentFish;
    
    public ObservableCollection<Fish> Fishes { get; } = new();
    // [<endsnippet DataFields>]

    public IRelayCommand NewCommand { get; }

    public IRelayCommand RemoveCommand { get; }

    public IRelayCommand EditCommand { get; }

    public IRelayCommand SaveCommand { get; }

    public IRelayCommand CancelCommand { get; }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty( ref _isEditing, value, true );
    }
    
    public Fish? CurrentFish
    {
        get => _currentFish;
        set => SetProperty( ref _currentFish, value, true );
    }

    // Design-time.
    public MainViewModel() : this( new FishGenerator( new RealNameGenerator()) ) { }

    public MainViewModel( IFishGenerator fishGenerator )
    {
        _fishGenerator = fishGenerator;

        NewCommand = new RelayCommand( ExecuteNew, CanExecuteNew );
        RemoveCommand = new RelayCommand( ExecuteRemove, CanExecuteRemove );
        EditCommand = new RelayCommand( ExecuteEdit, CanExecuteEdit );
        SaveCommand = new RelayCommand( ExecuteSave, CanExecuteSave );
        CancelCommand = new RelayCommand( ExecuteCancel, CanExecuteCancel );
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
        }
        else if (e.PropertyName == nameof(CurrentFish))
        {
            EditCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
        }

        base.OnPropertyChanged( e );
    }

    private void ExecuteNew()
    {
        Fishes.Add( new Fish() { Name = _fishGenerator.GetNewName(), Species = _fishGenerator.GetNewSpecies(), DateAdded = DateTime.Now } );
    }

    private bool CanExecuteNew()
    {
        return !IsEditing;
    }

    private void ExecuteRemove()
    {
        if (CurrentFish != null)
        {
            var index = Fishes.IndexOf( CurrentFish );
            Fishes.RemoveAt( index );

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

    private void ExecuteEdit()
    {
        IsEditing = true;
    }

    private bool CanExecuteEdit()
    {
        return CurrentFish != null && !IsEditing;
    }

    private void ExecuteSave()
    {
        IsEditing = false;
    }

    private bool CanExecuteSave()
    {
        return IsEditing;
    }

    private void ExecuteCancel()
    {
        IsEditing = false;
    }

    private bool CanExecuteCancel()
    {
        return IsEditing;
    }
}