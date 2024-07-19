namespace Memento.Step1;

public interface IMementoable
{
    IMemento SaveToMemento();

    void RestoreMemento( IMemento memento );
}