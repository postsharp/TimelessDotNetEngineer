﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Immutable;
using System.ComponentModel;

namespace Memento;

[Memento]
public partial class MainViewModel : ObservableRecipient
{
    private readonly ICaretaker _caretaker;
    private readonly IDataSource _dataSource;
    private bool _isEditing;
    private ItemViewModel? _currentItem;
    private ImmutableList<ItemViewModel> _items = ImmutableList<ItemViewModel>.Empty;

    public IRelayCommand NewCommand { get; }

    public IRelayCommand RemoveCommand { get; }

    public IRelayCommand EditCommand { get; }

    public IRelayCommand SaveCommand { get; }

    public IRelayCommand CancelCommand { get; }

    public IRelayCommand UndoCommand { get; }

    public bool IsEditing 
    { 
        get =>  this._isEditing; 
        set => this.SetProperty( ref this._isEditing, value, true ); 
    }

    public ImmutableList<ItemViewModel> Items 
    { 
        get => this._items; 
        private set => this.SetProperty( ref this._items, value, true ); 
    }

    public ItemViewModel? CurrentItem 
    { 
        get => this._currentItem; 
        set => this.SetProperty( ref this._currentItem, value, true ); 
    }

    public MainViewModel( IDataSource dataSource, ICaretaker caretaker )
    {
        this._dataSource = dataSource;
        this._caretaker = caretaker;

        this.NewCommand = new RelayCommand(this.ExecuteNew, this.CanExecuteNew);
        this.RemoveCommand = new RelayCommand( this.ExecuteRemove, this.CanExecuteRemove );
        this.EditCommand = new RelayCommand( this.ExecuteEdit, this.CanExecuteEdit );
        this.SaveCommand = new RelayCommand( this.ExecuteSave, this.CanExecuteSave );
        this.CancelCommand = new RelayCommand( this.ExecuteCancel, this.CanExecuteCancel );
        this.UndoCommand = new RelayCommand( this.ExecuteUndo, this.CanExecuteUndo );
    }

    protected override void OnPropertyChanged( PropertyChangedEventArgs e )
    {
        if ( e.PropertyName == nameof( this.IsEditing ) )
        {
            this.NewCommand.NotifyCanExecuteChanged();
            this.RemoveCommand.NotifyCanExecuteChanged();
            this.EditCommand.NotifyCanExecuteChanged();
            this.SaveCommand.NotifyCanExecuteChanged();
            this.CancelCommand.NotifyCanExecuteChanged();
            this.UndoCommand.NotifyCanExecuteChanged();
        }
        else if ( e.PropertyName == nameof( this.CurrentItem ) )
        {
            this.EditCommand.NotifyCanExecuteChanged();
            this.RemoveCommand.NotifyCanExecuteChanged();
            this.UndoCommand.NotifyCanExecuteChanged();
        }
        else if ( e.PropertyName == nameof( this.Items ) )
        {
            this.UndoCommand.NotifyCanExecuteChanged();
        }

        base.OnPropertyChanged( e );
    }

    private void ExecuteNew()
    {
        this._caretaker.Capture( this );

        this.Items = this.Items.Add( 
            new ItemViewModel()
            {
                Name = this._dataSource.GetNewName(),
                Species = this._dataSource.GetNewSpecies(),
                DateAdded = DateTime.Now,
            } );
    }

    private bool CanExecuteNew()
    {
        return !this.IsEditing;
    }

    private void ExecuteRemove()
    {
        if ( this.CurrentItem != null )
        {
            this._caretaker.Capture( this );

            var index = this.Items.IndexOf( this.CurrentItem );
            this.Items = this.Items.RemoveAt( index );

            if (index < this.Items.Count )
            {
                this.CurrentItem = this.Items[index];
            }
            else if ( this.Items.Count > 0 )
            {
                this.CurrentItem = this.Items[this.Items.Count - 1];
            }
            else
            {
                this.CurrentItem = null;
            }
        }
    }

    private bool CanExecuteRemove()
    {
        return this.CurrentItem != null && !this.IsEditing;
    }

    private void ExecuteEdit()
    {
        this.IsEditing = true;
        this._caretaker.Capture( this._currentItem! );
    }

    private bool CanExecuteEdit()
    {
        return this.CurrentItem != null && !this.IsEditing;
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
        this._caretaker.Undo();
    }

    private bool CanExecuteCancel()
    {
        return this.IsEditing;
    }

    private void ExecuteUndo()
    {
        this.IsEditing = false;

        // Remember the main list selection status before undo.
        var item = this.CurrentItem;
        var index = 
            item != null
            ? (int?)this.Items.IndexOf( item )
            : null;

        this._caretaker.Undo();

        // Fix the current item after undo.
        if (index != null)
        {
            if (index < this.Items.Count)
            {
                this.CurrentItem = this.Items[index.Value];
            }
            else if (this.Items.Count > 0)
            {
                this.CurrentItem = this.Items[this.Items.Count - 1];
            }
            else
            {
                this.CurrentItem = null;
            }
        }

        this.UndoCommand.NotifyCanExecuteChanged();
    }

    private bool CanExecuteUndo()
    {
        return this._caretaker.CanUndo;
    }
}