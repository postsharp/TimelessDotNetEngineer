// Copyright (c) SharpCrafters s.r.o. Released under the MIT License.

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
        get => this._isEditing;
        set => this.SetProperty( ref this._isEditing, value, true );
    }

    public Fish? CurrentFish
    {
        get => this._currentFish;
        set => this.SetProperty( ref this._currentFish, value, true );
    }

    // Design-time.
    public MainViewModel() : this( new FishGenerator( new RealNameGenerator() ) ) { }

    public MainViewModel( IFishGenerator fishGenerator )
    {
        this._fishGenerator = fishGenerator;

        this.NewCommand = new RelayCommand( this.ExecuteNew, this.CanExecuteNew );
        this.RemoveCommand = new RelayCommand( this.ExecuteRemove, this.CanExecuteRemove );
        this.EditCommand = new RelayCommand( this.ExecuteEdit, this.CanExecuteEdit );
        this.SaveCommand = new RelayCommand( this.ExecuteSave, this.CanExecuteSave );
        this.CancelCommand = new RelayCommand( this.ExecuteCancel, this.CanExecuteCancel );
    }

    protected override void OnPropertyChanged( PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == nameof(this.IsEditing) )
        {
            this.NewCommand.NotifyCanExecuteChanged();
            this.RemoveCommand.NotifyCanExecuteChanged();
            this.EditCommand.NotifyCanExecuteChanged();
            this.SaveCommand.NotifyCanExecuteChanged();
            this.CancelCommand.NotifyCanExecuteChanged();
        }
        else if ( e.PropertyName == nameof(this.CurrentFish) )
        {
            this.EditCommand.NotifyCanExecuteChanged();
            this.RemoveCommand.NotifyCanExecuteChanged();
        }

        base.OnPropertyChanged( e );
    }

    private void ExecuteNew()
    {
        this.Fishes.Add(
            new Fish()
            {
                Name = this._fishGenerator.GetNewName(),
                Species = this._fishGenerator.GetNewSpecies(),
                DateAdded = DateTime.Now
            } );
    }

    private bool CanExecuteNew()
    {
        return !this.IsEditing;
    }

    private void ExecuteRemove()
    {
        if ( this.CurrentFish != null )
        {
            var index = this.Fishes.IndexOf( this.CurrentFish );
            this.Fishes.RemoveAt( index );

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

    private void ExecuteEdit()
    {
        this.IsEditing = true;
    }

    private bool CanExecuteEdit()
    {
        return this.CurrentFish != null && !this.IsEditing;
    }

    private void ExecuteSave()
    {
        this.IsEditing = false;
    }

    private bool CanExecuteSave()
    {
        return this.IsEditing;
    }

    private void ExecuteCancel()
    {
        this.IsEditing = false;
    }

    private bool CanExecuteCancel()
    {
        return this.IsEditing;
    }
}