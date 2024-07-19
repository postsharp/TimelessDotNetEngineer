namespace Memento.Step2;

public interface IMementoable
{
    IMemento SaveToMemento();

    void RestoreMemento( IMemento memento );
}