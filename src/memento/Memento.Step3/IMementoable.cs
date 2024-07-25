namespace Memento.Step3;

public interface IMementoable
{
    IMemento SaveToMemento();

    void RestoreMemento( IMemento memento );
}